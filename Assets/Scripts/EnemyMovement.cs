/* EnemyMovement.cs */
using UnityEngine;

/// <summary>
/// Moves an enemy along a series of waypoints in order.
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    [Tooltip("Waypoints defining the path")]
    public Transform[] pathPoints;
    [Tooltip("Movement speed in units per second")]
    public float speed = 2f;

    private int currentPoint = 0;

    void Update()
    {
        if (pathPoints == null || pathPoints.Length == 0)
            return;

        // Move towards current waypoint
        Transform target = pathPoints[currentPoint];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Advance to next when close
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentPoint++;
            if (currentPoint >= pathPoints.Length)
            {
                // Reached end: destroy or trigger damage to player
                Destroy(gameObject);
            }
        }
    }
}