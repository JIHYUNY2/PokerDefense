using UnityEngine;
using System.Collections;

[System.Serializable]
public class Wave
{
    public GameObject enemyPrefab;
    public int count;
    public float spawnInterval = 1f;
}

/// <summary>
/// Controls spawning of waves of enemies using dynamically generated path points.
/// Spawns at the first path point (index 0) and waits for all enemies to complete looping.
/// </summary>
public class WaveManager : MonoBehaviour
{
    [Tooltip("Define each wave in sequence")]
    public Wave[] waves;

    [Tooltip("Reference to PathManager for path points")]
    public PathManager pathManager;

    private Transform[] pathPoints;
    private int currentWaveIndex = 0;

    public int CurrentWave => currentWaveIndex + 1;
    public int TotalWaves => waves.Length;

    IEnumerator Start()
    {
        // Wait until PathManager has generated path points
        yield return new WaitUntil(() => pathManager != null && pathManager.pathPoints != null && pathManager.pathPoints.Length > 0);

        pathPoints = pathManager.pathPoints;
        Debug.Log($"[WaveManager] Received {pathPoints.Length} path points, starting waves.");

        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            Wave wave = waves[currentWaveIndex];

            // Use top-left point (index 0) as spawn position
            Vector3 spawnPos = pathPoints[0].position;
            Debug.Log($"[WaveManager] Spawning wave {CurrentWave} at {spawnPos}");

            for (int i = 0; i < wave.count; i++)
            {
                GameObject enemy = Instantiate(wave.enemyPrefab, spawnPos, Quaternion.identity);
                var mover = enemy.GetComponent<EnemyMovement>();
                if (mover != null)
                    mover.pathPoints = pathPoints;

                yield return new WaitForSeconds(wave.spawnInterval);
            }

            // Wait until all enemies have finished looping
            yield return new WaitUntil(() =>
                UnityEngine.Object.FindObjectsByType<EnemyMovement>(FindObjectsSortMode.None).Length == 0
            );

            currentWaveIndex++;
        }

        Debug.Log("[WaveManager] All waves complete.");
    }
}