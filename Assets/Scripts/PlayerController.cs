using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 8f;
    private float horizontalInput;

    [Header("Salto y Gravedad")]
    public float jumpForce = 12f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Detección de Suelo")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Mecánicas Avanzadas (Metroidvania)")]
    [SerializeField] private float coyoteTime = 0.15f;
    private float coyoteTimeCounter;

    [SerializeField] private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Entrada de movimiento lateral (A/D o Flechas)
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 2. Detección de suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        // --- Coyote Time ---
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // --- Jump Buffer ---
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // 3. Ejecución del Salto con Buffer y Coyote Time
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0f;	// Consumir el buffer
            coyoteTimeCounter = 0f; // Consumir el coyote time
        }

        // Salto variable: si se suelta el botón de salto antes de tiempo, cae más rápido
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        // 4. Control de Gravedad Personalizada (Caídas más fluidas y menos flotantes)
        if (rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        // Aplicar velocidad de movimiento lateral manteniendo la física vertical
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    // Visualizar el radio del GroundCheck en el editor de Unity
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}