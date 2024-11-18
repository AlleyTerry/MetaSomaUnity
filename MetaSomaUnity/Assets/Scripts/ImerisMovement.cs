using System;
using UnityEngine;

public class ImerisMovement : MonoBehaviour
{
    // Variables for movement and jumping
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    
    private Rigidbody rb;
    [SerializeField] private bool isGrounded;
    private float groundCheckRadius = 0.3f;
    private Vector3 moveDirection;

    [SerializeField] private Animator playerAnimator = null;
    [SerializeField] private SpriteRenderer playerSpriteRender = null;

    [SerializeField] private bool jumpRequested;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerSpriteRender = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the rigidbody component for physics handling
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // Handle movement and jumping
        Move();
        Jump();
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
            playerSpriteRender.flipX = false;  // Facing right
        else if (rb.velocity.x < 0)
            playerSpriteRender.flipX = true;   // Facing left
    }

    private void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // Get input for left and right movement

        moveDirection = new Vector3(moveX, 0f, 0f);  // Set movement direction on the X-axis

        // Apply movement to the Rigidbody, maintaining Y velocity (gravity effects)
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }

    private void Jump()
    {
        // Handle jumping only if the player is grounded and the jump button is pressed
        if (isGrounded)
        {
            if (Input.GetButtonDown("Jump") && !jumpRequested)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0f);  // Apply jump force on Y axis
                jumpRequested = true;  // Prevent multiple jumps while airborne
            }
        }
        else
        {
            jumpRequested = false;  // Reset jump request when airborne
        }
    }

    // Draw the gizmo for ground detection in the scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
