using UnityEngine;

public class AutoRunPlayer : MonoBehaviour
{
    public float speed = 5f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(moveSpeed, 0f);
    }
}