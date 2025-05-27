/* UIManager.cs */
using UnityEngine;
using TMPro;

/// <summary>
/// Updates UI elements like current wave number.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Tooltip("TextMeshProUGUI for showing wave info")]
    public TMP_Text waveText;
    [Tooltip("Reference to the WaveManager")]
    public WaveManager waveManager;

    void Update()
    {
        if (waveManager != null && waveText != null)
        {
            waveText.text = $"Wave {waveManager.CurrentWave} / {waveManager.TotalWaves}";
        }
    }
}
