using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WalkableWalls : MonoBehaviour
{
    [Header("Wall Settings")]
    public bool isWalkable = false;
    public float wallMoveSpeed = 5f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip attachSound;
    public AudioClip detachSound;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private Rigidbody2D playerRigidbody;
    private ControllerInputSystem playerInput;
    private float originalGravity;
    private bool playerIsOnWall = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isWalkable || !other.CompareTag("Player")) return;

        playerRigidbody = other.GetComponent<Rigidbody2D>();
        playerInput = other.GetComponent<ControllerInputSystem>();

        if (playerRigidbody != null && playerInput != null)
        {
            originalGravity = playerRigidbody.gravityScale;
            playerRigidbody.gravityScale = 0f;
            playerRigidbody.velocity = Vector2.zero;
            playerIsOnWall = true;

            spriteRenderer.color = Color.red;

            PlaySound(attachSound);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isWalkable || !other.CompareTag("Player")) return;

        if (playerRigidbody != null)
        {
            playerRigidbody.gravityScale = originalGravity;
            playerRigidbody.velocity = Vector2.zero;
            playerIsOnWall = false;
            spriteRenderer.color = originalColor;

            PlaySound(detachSound);
        }

        playerRigidbody = null;
        playerInput = null;
    }

    private void FixedUpdate()
    {
        if (!playerIsOnWall || playerInput == null || playerRigidbody == null)
            return;

        float vertical = playerInput.MovementValue.y;
        Vector2 wallMovement = new Vector2(0f, vertical * wallMoveSpeed);
        playerRigidbody.velocity = wallMovement;
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}
