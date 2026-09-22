using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 3f;
    private bool movingRight = true;

    [Header("Detección de Vacíos y Paredes")]
    public Transform groundDetection;
    public float distance = 0.5f;
    public LayerMask whatIsGround;

    [Header("Combate / Daño")]
    public int damage = 1;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Mover al enemigo horizontalmente de forma constante
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

        // 2. Comprobar si hay suelo al frente con un Raycast
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance, whatIsGround);

        // 3. Si el raycast deja de tocar el suelo, giramos al enemigo
        if (groundInfo.collider == false)
        {
            Flip();
        }
    }

    void Flip()
    {
        movingRight = !movingRight;

        // Invertir la dirección de movimiento
        speed *= -1;

        // Invertir la escala visual del enemigo en el eje X para que mire hacia el otro lado
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    // Detectar colisión física con el jugador para causar daño
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                // AQUÍ ESTABA EL ERROR: Debes pasarle el daño Y la posición del enemigo
                playerHealth.TakeDamage(damage, transform.position);
            }
        }
    }

    // Visualizar el raycast de detección en la escena de Unity
    private void OnDrawGizmos()
    {
        if (groundDetection != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundDetection.position, groundDetection.position + Vector3.down * distance);
        }
    }
}