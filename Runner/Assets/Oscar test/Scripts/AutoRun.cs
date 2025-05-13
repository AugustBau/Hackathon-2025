using UnityEngine;

public class AutoRun : MonoBehaviour
{
    public static float moveSpeed = 5f;
    private Rigidbody2D rb;
    PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(moveSpeed, 0f);

        moveSpeed = 5f;
    }

    private void Update()
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocityY);
        }


  }

