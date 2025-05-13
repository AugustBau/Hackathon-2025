using UnityEngine;

public class PlayerSystem : Singleton<PlayerSystem>
{
    public float jumpForce = 50f;
    private Rigidbody2D rb;
    public bool isGrounded;
    private float slideTimer;
    bool isSliding = false;

    private Animator animator;


    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public BoxCollider2D playerCollider;
    private Vector2 originalColliderSize;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    private CapsuleCollider2D fallCheck;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        originalColliderSize = playerCollider.size;
        originalColliderSize.y = 0.64f;
        playerCollider.size = originalColliderSize;

        fallCheck = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
          if (isSliding == true)
            {
                animator.SetBool("isSliding", true);
            }
            else
            {
                animator.SetBool("isSliding", false);
            }
        
    }

    public void Jump ()
    {
        Debug.Log("jump");
        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        //rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        SoundManager.Instance.PlayPlayerSound("PlayerJump");
        
    }
    public void Slide()
    {

        StartCoroutine(SlideCoroutine());
        SoundManager.Instance.PlayPlayerSound("PlayerSlide");
    }

    private System.Collections.IEnumerator SlideCoroutine()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);

        Vector2 slideSize = playerCollider.size;
        slideSize.y = 0.15f;
        playerCollider.size = slideSize;

        yield return new WaitForSeconds(2f);

        playerCollider.size = originalColliderSize;
        isSliding = false;
        animator.SetBool("isSliding", false);
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            AutoRunPlayer.moveSpeed = 0f;
        }

        if (collision.CompareTag("Restart"))
        {
            SoundManager.Instance.PlayEnemySound("dying");
            GameManager.Instance.LoadScene("SampleScene");
        }
    }
}
