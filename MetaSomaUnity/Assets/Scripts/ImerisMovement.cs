using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImerisMovement : MonoBehaviour
{
    //variables :)
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    
    private Rigidbody rb;
    public bool isGrounded;
    private float groundCheckRadius = 0.2f;
    private Vector3 moveDirection;

    [SerializeField] private Animator playerAnimator = null;
    [SerializeField] private SpriteRenderer playerSpriteRender = null;
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerSpriteRender = GetComponent<SpriteRenderer>();
    }


    // Start is called before the first frame update
    void Start()
    {
        //get the rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
  
    }

    private void LateUpdate()
    {
        if (rb.velocity.x < 0f)
        {
            playerSpriteRender.flipX = true;
        }
        else if (rb.velocity.x > 0f)
        {
            playerSpriteRender.flipX = false;
        }
        
        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("xVelocity", MathF.Abs(rb.velocity.x));
            playerAnimator.SetFloat("yVelocity", rb.velocity.y);
            playerAnimator.SetBool("isGround", isGrounded);
        }
    }

    private void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); //left and right movement

        Vector3 movement = new Vector3(moveX, 0f, 0f);

        if (movement.x != 0)
        {
            Vector3 velocity = movement * moveSpeed;
            velocity.y = rb.velocity.y;
            rb.velocity = velocity;
        }

    }

    private void Jump()
    {
        //check if player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        
        //jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
    }
        
    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
