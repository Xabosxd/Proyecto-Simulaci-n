using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento Horizontal")]
    public float moveSpeed = 8f;
    private float horizontalInput;

    [Header("Física y Salto")]
    public Rigidbody2D rb;
    public float jumpForce = 16f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    [Header("Mecánicas Metroidvania (Game Feel)")]
    [SerializeField] private float coyoteTime = 0.15f;
    private float coyoteTimeCounter;

    [SerializeField] private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;

    private bool isFacingRight = true;

    void Update()
    {
        // Lectura de entrada de movimiento
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Detección de suelo
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Coyote Time
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump Buffer
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Ejecución del Salto con Buffer y Coyote Time
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumpingOrInAir())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f; // Previene saltos múltiples
        }

        // Altura de salto variable (si sueltas el botón antes de tiempo, frena el ascenso)
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }

        Flip();
    }

    void FixedUpdate()
    {
        // Movimiento horizontal fluido manteniendo la velocidad en Y
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private bool isJumpingOrInAir()
    {
        // Condición auxiliar para control interno de estados si se requiere
        return false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}