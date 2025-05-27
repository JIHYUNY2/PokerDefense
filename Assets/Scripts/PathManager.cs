/* PathManager.cs */
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Generates 4 corner path points around a UI grid using its RectTransform,
/// converting UI screen-space corners to world-space positions.
/// Includes debug logs for verification.
/// </summary>
public class PathManager : MonoBehaviour
{
    [Tooltip("RectTransform of the UI object containing all slots")]
    public RectTransform slotGridRect;

    [Tooltip("How far outside the grid corners (in world units) to place the path points")]
    public Vector2 offset = new Vector2(1f, 1f);

    [HideInInspector]
    public Transform[] pathPoints;

    /// <summary>
    /// Call this after slots are generated under slotGridRect to compute corner points.
    /// </summary>
    public void GeneratePathPoints()
    {
        Debug.Log("[PathManager] GeneratePathPoints called");
        if (slotGridRect == null)
        {
            Debug.LogError("[PathManager] slotGridRect is not assigned!");
            return;
        }

        // Get world-space corners of the UI RectTransform
        Vector3[] uiCorners = new Vector3[4];
        slotGridRect.GetWorldCorners(uiCorners);
        Debug.Log($"[PathManager] UI corners: BL{uiCorners[0]}, TL{uiCorners[1]}, TR{uiCorners[2]}, BR{uiCorners[3]}");

        // Convert each UI corner to screen space
        Vector3 screenBL = RectTransformUtility.WorldToScreenPoint(null, uiCorners[0]);
        Vector3 screenTL = RectTransformUtility.WorldToScreenPoint(null, uiCorners[1]);
        Vector3 screenTR = RectTransformUtility.WorldToScreenPoint(null, uiCorners[2]);
        Vector3 screenBR = RectTransformUtility.WorldToScreenPoint(null, uiCorners[3]);
        Debug.Log($"[PathManager] Screen corners: BL{screenBL}, TL{screenTL}, TR{screenTR}, BR{screenBR}");

        // Determine world-space Z depth for conversion (distance to z=0 plane)
        float worldZ = -Camera.main.transform.position.z;

        // Convert screen-space to world-space
        Vector3 worldBL = Camera.main.ScreenToWorldPoint(new Vector3(screenBL.x, screenBL.y, worldZ));
        Vector3 worldTL = Camera.main.ScreenToWorldPoint(new Vector3(screenTL.x, screenTL.y, worldZ));
        Vector3 worldTR = Camera.main.ScreenToWorldPoint(new Vector3(screenTR.x, screenTR.y, worldZ));
        Vector3 worldBR = Camera.main.ScreenToWorldPoint(new Vector3(screenBR.x, screenBR.y, worldZ));
        Debug.Log($"[PathManager] World corners: BL{worldBL}, TL{worldTL}, TR{worldTR}, BR{worldBR}");

        // Ensure array size
        if (pathPoints == null || pathPoints.Length != 4)
            pathPoints = new Transform[4];

        // Helper to create or reset a point transform
        System.Func<Transform, string, Vector3, Transform> setupPoint = (existing, name, pos) =>
        {
            if (existing == null)
            {
                GameObject go = new GameObject(name);
                go.transform.parent = transform;
                existing = go.transform;
            }
            existing.position = pos;
            Debug.Log($"[PathManager] {name} set to {pos}");
            return existing;
        };

        // Top-left (worldTL) offset upward-left
        pathPoints[0] = setupPoint(pathPoints[0], "PathPoint0", worldTL + new Vector3(-offset.x, offset.y, 0f));
        // Bottom-left
        pathPoints[1] = setupPoint(pathPoints[1], "PathPoint1", worldBL + new Vector3(-offset.x, -offset.y, 0f));
        // Bottom-right
        pathPoints[2] = setupPoint(pathPoints[2], "PathPoint2", worldBR + new Vector3(offset.x, -offset.y, 0f));
        // Top-right
        pathPoints[3] = setupPoint(pathPoints[3], "PathPoint3", worldTR + new Vector3(offset.x, offset.y, 0f));
    }
}