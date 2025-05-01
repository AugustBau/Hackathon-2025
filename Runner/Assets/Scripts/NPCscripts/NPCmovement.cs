using UnityEngine;

// Sørger for at der altid er en Rigidbody2D på objektet
[RequireComponent(typeof(Rigidbody2D))]
public class NPCFollower : MonoBehaviour
{
    // Hvor hurtigt NPC'en løber
    public float moveSpeed = 5f;

    // Hvor kraftigt NPC'en hopper
    public float jumpForce = 12f;

    // Hvor længe en slide varer
    public float slideDuration = 1f;

    private Animator animator;

    private Rigidbody2D rb;         // Reference til Rigidbody2D-komponenten
    private bool isGrounded = true; // Tjek om NPC'en er på jorden
    private bool isSliding = false; // Tjek om NPC'en er midt i en slide

    public Transform groundCheck;      // Et tomt GameObject under NPC'ens fødder
    public LayerMask groundLayer;      // Layer for jorden/platformene
    public LayerMask obstacleLayer;    // Layer for forhindringer (skorsten, vandtårn)

    private CapsuleCollider2D col;     // Reference til NPC'ens collider

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();     // Henter Rigidbody2D-komponenten
        col = GetComponent<CapsuleCollider2D>(); // Henter Collideren
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Så længe NPC'en ikke slider, skal den løbe fremad
        if (!isSliding)
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);

        // Tjek om vi står på jorden (lille cirkel under fødderne)
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Raycast lige frem (bruges til at opdage vandtårne → slide)
        RaycastHit2D frontHit = Physics2D.Raycast(transform.position, Vector2.right, 1f, obstacleLayer);

        // Raycast lidt frem + nedad (bruges til at opdage huller)
        Vector2 downOrigin = transform.position + Vector3.right * 0.6f;
        RaycastHit2D holeHit = Physics2D.Raycast(downOrigin, Vector2.down, 1.5f, groundLayer);

        // Raycast lidt oppe + fremad (bruges til at opdage skorstene)
        Vector2 upperOrigin = transform.position + Vector3.up * 0.6f;
        RaycastHit2D highHit = Physics2D.Raycast(upperOrigin, Vector2.right, 1f, obstacleLayer);

        // Hvis der er et hul ELLER en lav forhindring foran → hop (kun hvis vi står på jorden)
        if ((!holeHit.collider || highHit.collider) && isGrounded)
        {
            Jump();
        }

        // Hvis der er en høj forhindring lige foran → slide (kun hvis vi står på jorden og ikke allerede slider)
        if (frontHit.collider && !isSliding && isGrounded)
        {
            StartCoroutine(Slide());
        }
    }

    void Jump()
    {
        // Tilføj opadgående fart → hop
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    // Coroutine = noget der sker over tid
    System.Collections.IEnumerator Slide()
    {
        isSliding = true; // Nu slider vi
        animator.SetBool("isSliding", true);


        // Gør NPC'ens collider lavere (så den kan komme under vandtårnet)
        col.size = new Vector2(col.size.x, col.size.y / 2f);
        col.offset = new Vector2(col.offset.x, col.offset.y - 0.25f);

        // Vent i X sekunder
        yield return new WaitForSeconds(slideDuration);

        // Gør collider normal igen
        col.size = new Vector2(col.size.x, col.size.y * 2f);
        col.offset = new Vector2(col.offset.x, col.offset.y + 0.25f);

        isSliding = false; // Slut med slide
        animator.SetBool("isSliding", false);
    }

    // Denne metode tegner Raycasts i editoren, så du visuelt kan se hvad NPC'en "ser"
    void OnDrawGizmosSelected()
    {
        // Raycast lige frem (vandtårn)
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * 1f);

        // Raycast skråt ned (hul)
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + Vector3.right * 0.6f, (Vector2)transform.position + new Vector2(0.6f, -1.5f));

        // Raycast lidt oppe og frem (skorsten)
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.6f, (Vector2)transform.position + new Vector2(1f, 0.6f));
    }
}
