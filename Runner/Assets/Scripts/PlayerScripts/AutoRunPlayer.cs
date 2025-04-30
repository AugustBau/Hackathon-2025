using UnityEngine;
using System.Collections.Generic; // Importing the System.Collections.Generic namespace for using lists and collections
using System.Collections; // Importing the System.Collections namespace for using coroutines

public class AutoRun2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    void Update()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(moveSpeed, 0f);
        
    }
}
