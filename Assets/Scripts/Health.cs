using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Configuración de Salud")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Opcional")]
    public bool isPlayer = false;

    [Header("Configuración de Knockback (Retroceso)")]
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;
    private bool isKnockedBack = false;

    private Rigidbody2D rb;
    private EnemyPatrol enemyPatrol;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        enemyPatrol = GetComponent<EnemyPatrol>();
    }

    // Método mejorado para recibir daño y aplicar retroceso indicando la posición del atacante
    public void TakeDamage(int damageAmount, Vector2 damageSourcePosition)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " ha recibido daño. Vida restante: " + currentHealth);

        // Aplicar retroceso si tiene Rigidbody2D y no es el jugador (o si quieres que ambos retrocedan)
        if (rb != null)
        {
            StartCoroutine(ApplyKnockback(damageSourcePosition));
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator ApplyKnockback(Vector2 damageSourcePosition)
    {
        isKnockedBack = true;

        // Desactivar temporalmente el patrullaje del enemigo para que no pelee contra el retroceso
        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = false;
        }

        // Calcular la dirección del retroceso (alejándose de la fuente del daño)
        Vector2 direction = (transform.position - (Vector3)damageSourcePosition).normalized;

        // Aplicar fuerza de impulso repentina (asegurando un pequeño impulso vertical también)
        rb.linearVelocity = new Vector2(direction.x * knockbackForce, knockbackForce * 0.5f);

        yield return new WaitForSeconds(knockbackDuration);

        // Reactivar el movimiento de patrulla
        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = true;
        }

        isKnockedBack = false;
    }

    void Die()
    {
        if (isPlayer)
        {
            Debug.Log("¡El jugador ha muerto! Reiniciando nivel o checkpoint...");
        }
        else
        {
            Debug.Log(gameObject.name + " ha sido derrotado.");
            Destroy(gameObject);
        }
    }
}