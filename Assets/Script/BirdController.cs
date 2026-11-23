using UnityEngine;
using UnityEngine.InputSystem;

public class BirdController : MonoBehaviour
{
    [Header("Cài đặt bay")]
    public float jumpForce = 6.2f;
    public float maxHeight = 4.6f;

    [Header("Góc nghiêng")]
    public float tiltUpAngle = 30f;    // Hướng lên góc 30 độ khi tap
    public float tiltDownAngle = -30f; // Hướng xuống góc -30 độ khi không tap / rơi xuống
    public float tiltSmooth = 12f;

    [Header("Âm thanh Bird")]
    public AudioSource audioSource;
    public AudioClip jumpClip;      // Tiếng nhảy / vỗ cánh
    public AudioClip collisionClip; // Tiếng va chạm (ống / đất)

    private Rigidbody2D rb;
    private Animator anim;
    private float startY;
    private bool hasStarted = false;
    private bool isDead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        startY = transform.position.y;

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void BeginFlight()
    {
        if (isDead) return;
        hasStarted = true;

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        Jump();
    }

    void Update()
    {
        if (isDead) return;

        bool jumpInput = false;
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            jumpInput = true;
        if (!jumpInput && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            jumpInput = true;
        if (!jumpInput && Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            jumpInput = true;

        if (!hasStarted)
        {
            float hoverY = startY + Mathf.Sin(Time.time * 4f) * 0.12f;
            transform.position = new Vector3(transform.position.x, hoverY, transform.position.z);

            if (jumpInput && GameManager.instance != null)
            {
                GameManager.instance.StartGame();
            }
            return;
        }

        if (jumpInput)
        {
            Jump();
        }

        if (transform.position.y > maxHeight)
        {
            transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);
            if (rb != null && rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }

        RotateBird();
    }

    void Jump()
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0f, jumpForce);
        }

        if (audioSource != null && jumpClip != null)
        {
            audioSource.PlayOneShot(jumpClip);
        }
    }

    void RotateBird()
    {
        if (rb == null) return;

        float targetAngle = rb.linearVelocity.y > 0 ? tiltUpAngle : tiltDownAngle;
        float speedFactor = rb.linearVelocity.y > 0 ? tiltSmooth * 1.5f : tiltSmooth * 0.6f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speedFactor * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;
        isDead = true;

        if (audioSource != null && collisionClip != null)
        {
            audioSource.PlayOneShot(collisionClip);
        }

        if (anim != null) anim.enabled = false;
        if (GameManager.instance != null)
        {
            GameManager.instance.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;
        if (collision.CompareTag("Score") && GameManager.instance != null)
        {
            GameManager.instance.AddScore();
        }
    }
}