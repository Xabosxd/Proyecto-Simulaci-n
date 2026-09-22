using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Ataque Melee")]
    public Transform attackPoint;      // Un objeto hijo vacío situado frente al jugador
    public float attackRange = 0.5f;   // Radio del círculo de ataque
    public LayerMask enemyLayers;      // Capa asignada a los enemigos

    public int attackDamage = 1;
    public float attackRate = 0.5f;    // Tiempo de espera entre ataques
    private float nextAttackTime = 0f;

    void Update()
    {
        // Al presionar la tecla de ataque (por ejemplo, la tecla J o el Botón de Disparo/Acción)
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.J) || Input.GetButtonDown("Fire1"))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        // Opcional: Reproducir animación de ataque aquí

        // Detectar todos los enemigos en el rango de ataque usando un círculo solapado
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Intentar obtener el componente Health del enemigo golpeado
            Health enemyHealth = enemy.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage, transform.position);
            }
        }
    }

    // Visualizar el radio de ataque en el editor de Unity
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}