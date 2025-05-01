using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isSliding = false;

    public Transform groundCheck;
    public LayerMask groundLayer;

    public BoxCollider2D playerCollider;
    private Vector2 originalColliderSize;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        originalColliderSize = playerCollider.size;

        // Sæt collideren korrekt fra start
        originalColliderSize.y = 0.64f;
        playerCollider.size = originalColliderSize;
    }

    void Update()
    {
        // Brug Raycast til at tjekke om vi står på jorden
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.3f, groundLayer);
        isGrounded = hit.collider != null;

        Debug.Log("Is Grounded: " + isGrounded);

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

        Vector2 slideSize = playerCollider.size;
        slideSize.y = 0.1f;
        playerCollider.size = slideSize;

        yield return new WaitForSeconds(2f);

        playerCollider.size = originalColliderSize;
        isSliding = false;
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * 0.3f);
        }
    }
}
