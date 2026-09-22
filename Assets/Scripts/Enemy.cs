using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    [Header("Movimiento y Patrulla")]
    public float speed = 3f;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;
    public float checkDistance = 0.5f;

    private Rigidbody2D rb;
    private bool movingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Detección de suelo frente a la dirección actual para no caer al vacío
        bool isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, checkDistance, groundLayer);

        // Detección de pared frontal para girar al chocar
        bool hitWall = Physics2D.Raycast(wallCheck.position, movingRight ? Vector2.right : Vector2.left, checkDistance, groundLayer);

        // Si llega al borde del abismo o choca con una pared, cambia de dirección
        if (!isGrounded || hitWall)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        // Movimiento horizontal constante según la dirección
        float moveDirection = movingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(moveDirection * speed, rb.linearVelocity.y);
    }

    void Flip()
    {
        movingRight = !movingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Daño al jugador por contacto
        if (collision.gameObject.CompareTag("Player"))
        {
            // Aquí puedes llamar al método de recibir daño del jugador, ej: collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
            Debug.Log("El enemigo hizo daño al jugador por contacto.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * checkDistance);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (movingRight ? Vector3.right : Vector3.left) * checkDistance);
        }
    }
}