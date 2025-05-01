using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private float slideTimer;
    bool isSliding = false;

    private Animator animator;


    public Transform groundCheck;
    public LayerMask groundLayer;

    public BoxCollider2D playerCollider;
    private Vector2 originalColliderSize;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        originalColliderSize = playerCollider.size;

        // Sæt collideren korrekt fra start
        originalColliderSize.y = 0.64f;
        playerCollider.size = originalColliderSize;
    }

    void Update()
    {
        
        float swipeThreshold = 50f; // Adjust for sensitivity

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    startTouchPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    endTouchPosition = touch.position;
                    Vector2 swipeDirection = endTouchPosition - startTouchPosition;

                if (Mathf.Abs(swipeDirection.y) > swipeThreshold && Mathf.Abs(swipeDirection.y) > Mathf.Abs(swipeDirection.x))
                {
                    if (swipeDirection.y > 0 && isGrounded)
                    {
                        // Swipe Up - Jump
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                        SoundManager.Instance.PlayPlayerSound("PlayerJump");
                    }
                    else if (swipeDirection.y < 0 && isGrounded && !isSliding)
                    {
                        // Swipe Down - Slide
                        StartCoroutine(SlideCoroutine());
                        SoundManager.Instance.PlayPlayerSound("PlayerSlide");
                    }
                }
            }
        }

    }

    private System.Collections.IEnumerator SlideCoroutine()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);

        Vector2 slideSize = playerCollider.size;
        slideSize.y = 0.3f;
        playerCollider.size = slideSize;

        yield return new WaitForSeconds(2f);

        playerCollider.size = originalColliderSize;
        isSliding = false;
        animator.SetBool("isSliding", false);
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
