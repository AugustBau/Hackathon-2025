using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private bool isGrounded;


    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         
       rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }


    void OnDrawGizmos()
    {
        if (groundCheck != null)
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
