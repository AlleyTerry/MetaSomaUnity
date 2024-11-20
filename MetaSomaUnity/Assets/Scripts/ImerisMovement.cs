using System;
using UnityEngine;

public enum Evolution
{
    None,
    First,
    Fully
}

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
    [SerializeField] private Evolution evolutionState = Evolution.None;
    
    public Evolution EvolutionState
    {
        get
        {
            return evolutionState;
        }
        set
        {
            evolutionState = value;
        }
    }
    
    // RIGIDBODY COMPONENT
    private Rigidbody rb;
    
    // MOVEMENT VARIABLES
    public float moveSpeed = 5f;
    private Vector3 moveDirection;
    
    // JUMP VARIABLES
    public Transform groundCheck;
    public LayerMask groundLayer;
    
    [SerializeField] private bool isGrounded;
    private float groundCheckRadius = 0.3f;
    
    public float jumpForce = 5f;
    
    // ... slowing down the falling speed
    private bool isJumping = false;
    public float jumpSlowdownFalling = 0.5f;

    // ANIMATOR AND SPRITE RENDERER
    [SerializeField] private Animator playerAnimator = null;
    [SerializeField] private SpriteRenderer playerSpriteRender = null;
    
    // Start is called before the first frame update
    void Start()
    {
        // Wiring up components
        playerAnimator = GetComponent<Animator>();
        playerSpriteRender = GetComponent<SpriteRenderer>();
        
        // Get the rigidbody component for physics handling
        rb = GetComponent<Rigidbody>();
        
        // Initialize the evolution state
        EvolutionState = Evolution.None;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // Handle movement and jumping
        Move();
        JumpHandler();
    }

    private void LateUpdate()
    {
        // Update the animator parameters
        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
            playerAnimator.SetFloat("yVelocity", rb.velocity.y);
            playerAnimator.SetBool("isGround", isGrounded);
        }

        // Flip the sprite based on movement direction
        if (rb.velocity.x > 0)
            playerSpriteRender.flipX = true;  // Facing right
        else if (rb.velocity.x < 0)
            playerSpriteRender.flipX = false;   // Facing left
    }

    private void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // Get input for left and right movement

        moveDirection = new Vector3(moveX, 0f, 0f);  // Set movement direction on the X-axis

        // Apply movement to the RigidBody, maintaining Y velocity (gravity effects)
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }

    private void JumpHandler()
    {
        // Handle jumping only if the player is grounded and the jump button is pressed
        if (evolutionState == Evolution.None)
        {
            // Default jump
            Evolution0Jump();
        }
        else if (evolutionState == Evolution.First)
        {
            // Evolution 1 jump, jump higher and land slower
            Evolution1Jump();
        }
        else if (evolutionState == Evolution.Fully)
        {
            
        }
    }

    // Evolution 0 Jump
    private void Evolution0Jump()
    {
        if (isGrounded &&
            Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0f);  // Apply jump force on Y axis
        }
    }
    
    // Evolution 1 Jump
    private void Evolution1Jump()
    {
        if (isGrounded &&
            Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce * 1.15f, 0f);  // Apply jump force on Y axis
            isJumping = true;
        }
        else if (!isGrounded &&
                 Input.GetButton("Jump") &&
                 isJumping &&
                 rb.velocity.y < 0)
        {
            // Apply slow fall effect when falling and holding jump
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * jumpSlowdownFalling, 0f);  // Slow down the falling speed
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }
    
    // Fully evolved jump
    private void Evolution2Jump()
    {
        
    }
    
    // Draw the gizmo for ground detection in the scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
