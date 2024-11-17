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
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        //get the rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //check if player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        //isGrounded = Physics.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        //get the input from the player
        float horizontalInput = Input.GetAxis("Horizontal");
        moveDirection = new Vector3(horizontalInput, 0);
        
        //update movement
        if (moveDirection.x != 0)
        {
            rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y);
        }
        
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
