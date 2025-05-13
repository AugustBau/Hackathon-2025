using UnityEngine;
using UnityEngine.SceneManagement;

// Sørger for at der altid er en Rigidbody2D på objektet
[RequireComponent(typeof(Rigidbody2D))]
public class NPCFollower : MonoBehaviour
{
    // Bevægelse
    public float moveSpeed = 5f;
    private float originalSpeed;
    public float slowedSpeed = 2f;
    public float slowDuration = 2f;

    // Hop og slide
    public float jumpForce = 12f;
    public float slideDuration = 1f;
    private bool isSliding = false;

    // Ground check
    private Rigidbody2D rb;
    private bool isGrounded = true;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;

    // Collider
    private CapsuleCollider2D col;
    private BoxCollider2D playerTouch;
    private bool isTouchingPlayer = false;

    // Shake detection
    public float shakeThreshold = 2.0f;
    public float shakeCooldown = 1.0f;
    private float lastShakeTime = 0f;


    private bool canJumpAgain = true;
    private bool hasJumped = false;
    private bool wasOverHoleLastFrame = false;
    private Animator animator;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        playerTouch = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        isTouchingPlayer = false;
        originalSpeed = moveSpeed;
    }

    void Update()
    {
        if (!isSliding)
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Raycast lige frem for at tjekke WaterTank og Ventilator
        Vector2 frontOrigin = transform.position + Vector3.up * 0.3f;
        RaycastHit2D frontHit = Physics2D.Raycast(frontOrigin, Vector2.right, 2f, obstacleLayer);
        Debug.DrawRay(frontOrigin, Vector2.right * 2f, Color.red);
        Vector2 downOrigin = transform.position + Vector3.right * 0.6f;
        RaycastHit2D holeHit = Physics2D.Raycast(downOrigin, Vector2.down, 1.5f, groundLayer);
        Vector2 upperOrigin = transform.position + Vector3.up * 0.6f;
        RaycastHit2D highHit = Physics2D.Raycast(upperOrigin, Vector2.right, 1f);

        // Tjek om vi skal hoppe pga. hul eller lav forhindring
        bool isOverHole = !holeHit.collider;
        bool shouldJump = isOverHole && !wasOverHoleLastFrame && isGrounded;

        if (shouldJump)
        {
            Jump();
        }

        wasOverHoleLastFrame = isOverHole;


        if (holeHit.collider && isGrounded)
        {
            hasJumped = false;
        }

        if (Time.time - lastShakeTime > shakeCooldown)
        {
            Vector3 acceleration = Input.acceleration;
            if (acceleration.sqrMagnitude >= shakeThreshold * shakeThreshold)
            {
                lastShakeTime = Time.time;
                StartCoroutine(SlowDownNPC());
            }
        }
    }

    bool IsLayer(GameObject obj, string layerName)
    {
        return obj.layer == LayerMask.NameToLayer(layerName);
    }


    void Jump()
    {
        Debug.Log("Jump detected");
        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        SoundManager.Instance.PlayEnemySound("jumping");

        hasJumped = true;
    }

    System.Collections.IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        col.size = new Vector2(col.size.x, col.size.y / 2f);
        col.offset = new Vector2(col.offset.x, col.offset.y - 0.25f);
        yield return new WaitForSeconds(slideDuration);
        col.size = new Vector2(col.size.x, col.size.y * 2f);
        col.offset = new Vector2(col.offset.x, col.offset.y + 0.25f);
        isSliding = false;
        animator.SetBool("isSliding", false);
    }

    System.Collections.IEnumerator SlowDownNPC()
    {
        moveSpeed = slowedSpeed;
        Debug.Log("Shaking detected");
        yield return new WaitForSeconds(slowDuration);
        moveSpeed = originalSpeed;
        SoundManager.Instance.PlayEnemySound("shaken");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * 1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + Vector3.right * 0.6f, (Vector2)transform.position + new Vector2(0.6f, -1.5f));

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.6f, (Vector2)transform.position + new Vector2(1f, 0.6f));
    }

     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTouchingPlayer)
        {
            if (other.GetComponent<PlayerMovement>().MegaMode == false)
            {
                SoundManager.Instance.PlayEnemySound("catching");
                isTouchingPlayer = true;
                Debug.Log("touching player");
                GameManager.Instance.LoadScene("SampleScene");
            }
            else
            {
                transform.position += new Vector3(-4f, 0.5f, 0);

            }
        }

        // Slide under WaterTank
        if (other.gameObject.layer == LayerMask.NameToLayer("WaterTank") && !isSliding && isGrounded)
        {
            Debug.Log("Slide detected");
            StartCoroutine(Slide());
            SoundManager.Instance.PlayEnemySound("jumping");
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")  && isGrounded)
        {
            Jump();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ventilator") && isGrounded)
        {
            Jump();
        }



    }

}
