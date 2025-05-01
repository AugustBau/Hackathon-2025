using UnityEngine;

public class AutoRunPlayer : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(moveSpeed, 0f);
    }

    private void Update()
    { if (playerMovement.isGrounded)
        {
            // Keep the player moving forward
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocityY);
        }

    }
}
