using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private float slideTimer;
    bool isSliding = false;


    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public BoxCollider2D playerCollider;
    private Vector2 originalColliderSize;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        originalColliderSize = playerCollider.size;
        originalColliderSize.y = 0.64f;
        playerCollider.size = originalColliderSize;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.S) && isGrounded && !isSliding)
        {
            StartCoroutine(SlideCoroutine());
        }
    }

    private System.Collections.IEnumerator SlideCoroutine()
    {
        isSliding = true;

        // Shrink collider height and lower it slightly
        Vector2 slideSize = playerCollider.size;
        Vector2 slideOffset = playerCollider.offset;

        slideSize.y = 0.3f; // Don't go too low — try half the original height
        slideOffset.y -= 0.15f; // Move it down so bottom stays on the ground

        playerCollider.size = slideSize;
        playerCollider.offset = slideOffset;

        yield return new WaitForSeconds(1.5f); // Adjust duration

        // Reset to original size and offset
        playerCollider.size = originalColliderSize;
        playerCollider.offset = Vector2.zero; // Assuming original was centered
        isSliding = false;
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
