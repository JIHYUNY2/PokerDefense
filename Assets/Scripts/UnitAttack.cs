/* UnitAttack.cs */
using UnityEngine;

/// <summary>
/// Attach to unit prefab. Deals damage to enemies in range at intervals.
/// Requires a CircleCollider2D (isTrigger) to define range.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class UnitAttack : MonoBehaviour
{
    [Tooltip("Damage per attack")] public float damage = 10f;
    [Tooltip("Time in seconds between attacks")] public float attackInterval = 1f;
    private float cooldown = 0f;

    void Update()
    {
        cooldown -= Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (cooldown <= 0f)
        {
            var health = other.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
                cooldown = attackInterval;
            }
        }
    }
}