using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Yarn.Unity;

public class ImerisMovement : MonoBehaviour
{
    // SINGLETON
    public static ImerisMovement instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    // EVOLUTION STATE
    public CharacterState currentState;
    
    // RIGIDBODY COMPONENT
    public Rigidbody rb;
    
    // SPAWN POINT
    public Vector3 spawnPosition;
    public bool isFacingRight = false;
    
    // MOVEMENT VARIABLES
    public float moveSpeed = 5f;
    private Vector3 moveDirection;
    
    // JUMP VARIABLES
    public bool jumpEnabled = true;
    [SerializeField] private Transform groundCheck;
    private LayerMask groundLayer;
    
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckRadius = 0.05f;
    
    // STEPPABLE OBJECTS CHECK
    [SerializeField] private bool isOnSteppable = false;
    [SerializeField] private LayerMask steppableLayer;
    
    public float jumpForce = 5f;
    private bool hasDoubleJumped = false;
    
    // ... slowing down the falling speed
    [SerializeField]private bool isJumping = false;
    public float jumpSlowdownFalling = 0.5f;

    // ANIMATOR AND SPRITE RENDERER
    [SerializeField] private Animator playerAnimator = null;
    [SerializeField] private SpriteRenderer playerSpriteRender = null;
    
    // NOTE: THIS IS FOR TESTING AND DEBUGGING PURPOSES ONLY, COMMENT OUT IN FINAL BUILD
    public enum SetStateButton
    {
        Evo0WetWings,
        Evo0Hungry,
        Evo0Healthy,
        Evo1WetWings,
        Evo1Hungry,
        Evo1Healthy,
        Evo2WetWings,
        Evo2Hungry,
        Evo2Healthy
    }
    [SerializeReference] private SetStateButton setStateButton;

    private void OnValidate()
    {
        UpdateStateFromButton();
    }

    private void UpdateStateFromButton()
    {
        switch (setStateButton)
        {
            case SetStateButton.Evo0WetWings:
                SetState(new BeforeAnyEvolutionState(SubState.WetWings));
                break;
            case SetStateButton.Evo0Hungry:
                SetState(new BeforeAnyEvolutionState(SubState.Hungry));
                break;
            case SetStateButton.Evo0Healthy:
                SetState(new BeforeAnyEvolutionState(SubState.Healthy));
                break;
            case SetStateButton.Evo1WetWings:
                SetState(new EatenLAndEvolvedState(SubState.WetWings));
                break;
            case SetStateButton.Evo1Hungry:
                SetState(new EatenLAndEvolvedState(SubState.Hungry));
                break;
            case SetStateButton.Evo1Healthy:
                SetState(new EatenLAndEvolvedState(SubState.Healthy));
                break;
            case SetStateButton.Evo2WetWings:
                SetState(new NoLEvolvedFinalState(SubState.WetWings));
                break;
            case SetStateButton.Evo2Hungry:
                SetState(new NoLEvolvedFinalState(SubState.Hungry));
                break;
            case SetStateButton.Evo2Healthy:
                SetState(new NoLEvolvedFinalState(SubState.Healthy));
                break;
        }
    }
    
    /*public void SetSpawnPosition(Vector3 newSpawnPosition, bool isFacingRight)
    {
        rb.isKinematic = true;
        
        transform.position = newSpawnPosition;
        GetComponent<SpriteRenderer>().flipX = isFacingRight;
        
        rb.velocity = Vector3.zero;
        
        StartCoroutine(EnablePhysics());
    }

    private IEnumerator EnablePhysics()
    {
        yield return new WaitForSeconds(0.1f);
        rb.isKinematic = false;
    }*/

    // Start is called before the first frame update
    void Start()
    {
        // Wiring up components
        playerAnimator = GetComponent<Animator>();
        playerSpriteRender = GetComponent<SpriteRenderer>();

        // Setting up the ground check
        groundCheck = transform.GetChild(0);  // Get the ground check object
        groundLayer = LayerMask.GetMask("GroundLayer");  // Get the ground layer
        
        // Get the rigidbody component for physics handling
        rb = GetComponent<Rigidbody>();
        
        // Initialize the evolution state
        currentState = new BeforeAnyEvolutionState(SubState.Healthy); // Commented out for testing purposes
        
        // INITIALIZE THE PLAYER ANIMATOR
        playerAnimator.Play("Imeris_Cloak_Idle");
        
        // TODO: THIS IS THE JUMPING PART
        // Constraint the Y position of the player, and all rotations
        DisableJump();
        
        // Set isFacingRight
        isFacingRight = !playerSpriteRender.flipX;
    }

    // Update is called once per frame
    void Update()
    {
        //test for jump functions
        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            EnableJump();
        }*/
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // Handle movement and jumping
        if (!GameManager.instance.isInBattle)
        {
            Move();
            if (jumpEnabled) JumpHandler();
        }
        
        // todo: need to be optimized
        if (!jumpEnabled && isGrounded && GameManager.instance.isHoldingChapelKey)
        {
            ConstraintYPosition();
        }
    }

    private void LateUpdate()
    {
        /*// Update the animator parameters
        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
            playerAnimator.SetFloat("yVelocity", rb.velocity.y);
            playerAnimator.SetBool("isGround", isGrounded);
        }*/

        // Walk Animation
        if (rb.velocity.x != 0 &&
            (isGrounded || isOnSteppable))
        {
            playerAnimator.Play("Imeris_Cloak_Walk");
        }
        else if (rb.velocity.x == 0 &&
                 (isGrounded || isOnSteppable))
        {
            playerAnimator.Play("Imeris_Cloak_Idle");
        }

        // Jump Animation

        // Flip the sprite based on movement direction
        if (rb.velocity.x > 0)
        {
            isFacingRight = true;
            playerSpriteRender.flipX = true; // Facing right
        }
        else if (rb.velocity.x < 0)
        {
            isFacingRight = false;
            playerSpriteRender.flipX = false; // Facing left
        }
    }

    private void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // Get input for left and right movement

        moveDirection = new Vector3(moveX, 0f, 0f);  // Set movement direction on the X-axis

        // Apply movement to the RigidBody, maintaining Y velocity (gravity effects)
        Vector3 velocity = moveDirection * (moveSpeed * currentState.GetWalkSpeed());
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
        
        float actualSpeed = moveSpeed * currentState.GetWalkSpeed();
        //Debug.Log($"Current Walk Speed: {actualSpeed}");
    }

    private void JumpHandler()
    {
        // Handle jumping 
        if ((isGrounded || isOnSteppable) && 
            (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space)))
        {
            // Apply jump force on Y axis
            rb.velocity = new Vector3(rb.velocity.x, jumpForce * currentState.GetJumpHeight(), 0f);  
            isJumping = true;
            hasDoubleJumped = false;
            
            playerAnimator.Play("Player_Jump");
            Debug.Log("IsJumping");
        }
        // Double jump
        else if ((!isGrounded || !isOnSteppable) && 
                 currentState.CanDoubleJump() &&
                 (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space)) &&
                 !hasDoubleJumped)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce * currentState.GetJumpHeight() * 0.9f, 0f);  // Apply jump force on Y axis
            hasDoubleJumped = true;
            isJumping = true;
            
            playerAnimator.Play("Player_Jump");
            Debug.Log("Double Jump");
        }
        
        // Floating
        if ((!isGrounded || !isOnSteppable) && 
            isJumping &&
            hasDoubleJumped &&
            currentState.CanFloat() &&
            (Input.GetButton("Jump") || Input.GetKeyDown(KeyCode.Space)) &&
            rb.velocity.y < 0)
        {
            // Apply slow fall effect when falling and holding jump
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * jumpSlowdownFalling, 0f);  // Slow down the falling speed
        }
        
        if (Input.GetButtonUp("Jump") || Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = false;
            /*hasDoubleJumped = false;*/
        }
    }
    
    // Draw the gizmo for ground detection in the scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    
    private void SetState(CharacterState newState)
    {
        currentState = newState;
    }
    
    public void SetSubState(SubState newSubState)
    {
        if (currentState is BeforeAnyEvolutionState beforeAnyEvolutionState)
        {
            beforeAnyEvolutionState.SetSubState(newSubState);
            Debug.Log("SubState changed to: " + newSubState);
        }
        else if (currentState is EatenLAndEvolvedState eatenLAndEvolvedState)
        {
            eatenLAndEvolvedState.SetSubState(newSubState);
            Debug.Log("SubState changed to: " + newSubState);
        }
        else if (currentState is NoLEvolvedFinalState noLEvolvedFinalState)
        {
            noLEvolvedFinalState.SetSubState(newSubState);
            Debug.Log("SubState changed to: " + newSubState);
        }
    }
    
    // GET INTO CUTSCENE (BATTLE)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BattleTrigger"))
        {
            BattleTrigger battleTrigger = other.gameObject.GetComponent<BattleTrigger>();

            if (battleTrigger != null && 
                !battleTrigger.IsTriggered)
            {
                battleTrigger.FlagTriggerUsed();
                GameManager.instance.isInBattle = true;
                
                rb.velocity = Vector3.zero;
                
                // TODO: NOTE -- if the current level manager is not set, try other way, or check the GM
                GameManager.instance.currentLevelManager.RegisterCutSceneAndBattle
                    (battleTrigger.cutSceneDialogueNode, battleTrigger.battleDialogueNode, battleTrigger.battleDialogueDelay);
                
                other.gameObject.SetActive(false); // Disable the trigger
            }
            
            /*GameManager.instance.currentLevelManager.BattleScene();*/
            /*GameManager.instance.currentLevelManager.StartCutsScene
                (other.gameObject.GetComponent<BattleTrigger>().cutSceneDialogueNode);
            
            other.gameObject.SetActive(false); // Disable the trigger*/
        }
    }
    
    // EXIT BATTLE
    [YarnCommand("ExitBattle")]
    public void ExitBattle()
    {
        Debug.Log("Exiting battle...");
        GameManager.instance.isInBattle = false;
        
        GameManager.instance.currentLevelManager.ExitBattleDialogue();
    }
    
    // Stair fixing
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Stairs"))
        {
            UnconstraintYPosition();
        }

        if (IsInLayerMask(other.gameObject, steppableLayer))
        {
            isOnSteppable = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Stairs"))
        {
            ConstraintYPosition();
        }
        
        if (IsInLayerMask(other.gameObject, steppableLayer))
        {
            isOnSteppable = false;
        }
    }

    // CHECK IF THE COLLISION IS IN THE LAYER MASK
    private bool IsInLayerMask(GameObject gameObject, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << gameObject.layer)) > 0);
    }

    [YarnCommand("DisableJump")]
    public void DisableJump()
    {
        jumpEnabled = false;
        if (!jumpEnabled && isGrounded)
        {
            ConstraintYPosition();
        }
    }

    [YarnCommand("EnableJump")]
    public void EnableJump()
    {
        jumpEnabled = true;
        UnconstraintYPosition();
    }
  
    private void ConstraintYPosition()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY | 
                         RigidbodyConstraints.FreezePositionZ | 
                         RigidbodyConstraints.FreezeRotationX | 
                         RigidbodyConstraints.FreezeRotationY |
                         RigidbodyConstraints.FreezeRotationZ;
    }
    
    private void UnconstraintYPosition()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionZ | 
                         RigidbodyConstraints.FreezeRotationX | 
                         RigidbodyConstraints.FreezeRotationY |
                         RigidbodyConstraints.FreezeRotationZ;
    }
}
