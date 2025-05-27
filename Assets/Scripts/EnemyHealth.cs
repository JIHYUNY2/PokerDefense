/* EnemyHealth.cs */
using UnityEngine;

/// <summary>
/// Simple health component for enemies.
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    [Tooltip("Maximum health points")]
    public float maxHealth = 50f;

    private float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Apply damage to this enemy. Destroys object if health falls to zero.
    /// </summary>
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        // TODO: play death effect
        Destroy(gameObject);
    }
}