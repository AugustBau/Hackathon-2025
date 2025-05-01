using UnityEngine;

public class AutoRunPlayer : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(moveSpeed, 0f);
    }

    private void Update()
    {
        // Keep the player moving forward
        rb.linearVelocity = new Vector2(moveSpeed, 0f);
    }
}
