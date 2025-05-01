using UnityEngine;

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

    // UI og grafik
    public GameObject restartButton;
    public SpriteRenderer player;
    public SpriteRenderer enemy;

    // Shake detection
    public float shakeThreshold = 2.0f;
    public float shakeCooldown = 1.0f;
    private float lastShakeTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        playerTouch = GetComponent<BoxCollider2D>();

        isTouchingPlayer = false;
        originalSpeed = moveSpeed;

        player.enabled = true;
        enemy.enabled = true;
    }

    void Update()
    {
        // Bevæg NPC'en hvis ikke den slider
        if (!isSliding)
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocityY);

        // Tjek om NPC'en står på jorden
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Raycasts for at opdage forhindringer
        RaycastHit2D frontHit = Physics2D.Raycast(transform.position, Vector2.right, 1f, obstacleLayer);
        Vector2 downOrigin = transform.position + Vector3.right * 0.6f;
        RaycastHit2D holeHit = Physics2D.Raycast(downOrigin, Vector2.down, 1.5f, groundLayer);
        Vector2 upperOrigin = transform.position + Vector3.up * 0.6f;
        RaycastHit2D highHit = Physics2D.Raycast(upperOrigin, Vector2.right, 1f, obstacleLayer);

        // Hop ved hul eller skorsten
        if ((!holeHit.collider || highHit.collider) && isGrounded)
        {
            Jump();
        }

        // Slide ved lav forhindring
        if (frontHit.collider && !isSliding && isGrounded)
        {
            Debug.Log("Daniel sldies");
            StartCoroutine(Slide());
        }

        // Shake detection
        if (Time.time - lastShakeTime > shakeCooldown)
        {
            if (Input.acceleration.sqrMagnitude >= shakeThreshold * shakeThreshold)
            {
                lastShakeTime = Time.time;
                StartCoroutine(SlowDownNPC());
            }
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
    }

    System.Collections.IEnumerator Slide()
    {
        isSliding = true;
        col.size = new Vector2(col.size.x, col.size.y / 2f);
        col.offset = new Vector2(col.offset.x, col.offset.y - 0.25f);
        yield return new WaitForSeconds(slideDuration);
        col.size = new Vector2(col.size.x, col.size.y * 2f);
        col.offset = new Vector2(col.offset.x, col.offset.y + 0.25f);
        isSliding = false;
    }

    System.Collections.IEnumerator SlowDownNPC()
    {
        moveSpeed = slowedSpeed;    
        Debug.Log("Shaking detected");
        yield return new WaitForSeconds(slowDuration);
        moveSpeed = originalSpeed;
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
            SoundManager.Instance.PlayEnemySound("catching");
            isTouchingPlayer = true;
            Debug.Log("touching player");
            player.enabled = false;
            enemy.enabled = false;
            restartButton.SetActive(true);
        }
    }
}
