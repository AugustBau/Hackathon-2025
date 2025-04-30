using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    public float runSpeed = 5f;
    public float jumpForce = 8f;
    public LayerMask groundLayer;
    public LayerMask slideAreaLayer; // ⬅ NEW
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public CapsuleCollider2D slideCollider;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isSliding = false;

    private Vector2 startTouchPos;
    private Vector2 endTouchPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Auto-run to the right
        rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y);

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Optional: Auto-slide if entering a slide area
        bool isInSlideArea = Physics2D.OverlapCircle(transform.position, 0.3f, slideAreaLayer);
        if (isInSlideArea && isGrounded && !isSliding)
        {
            StartCoroutine(Slide());
        }

        HandleInput();
    }

    void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Tap to jump
            if (touch.phase == TouchPhase.Began)
            {
                startTouchPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPos = touch.position;
                float swipeDistance = endTouchPos.y - startTouchPos.y;

                if (Mathf.Abs(swipeDistance) < 30f)
                {
                    // Tap = Jump
                    if (isGrounded)
                        Jump();
                }
                else if (swipeDistance < -50f)
                {
                    // Swipe down = Slide
                    if (isGrounded && !isSliding)
                        StartCoroutine(Slide());
                }
            }
        }

#if UNITY_EDITOR
        // Jump with space in editor
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            Jump();
#endif
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    System.Collections.IEnumerator Slide()
    {
        if (isSliding) yield break;
        isSliding = true;

        // Reduce collider height
        slideCollider.offset = new Vector2(slideCollider.offset.x, slideCollider.offset.y - 0.3f);
        slideCollider.size = new Vector2(slideCollider.size.x, slideCollider.size.y - 0.6f);

        yield return new WaitForSeconds(0.8f);

        // Reset collider
        slideCollider.offset = new Vector2(slideCollider.offset.x, slideCollider.offset.y + 0.3f);
        slideCollider.size = new Vector2(slideCollider.size.x, slideCollider.size.y + 0.6f);

        isSliding = false;
    }
}
