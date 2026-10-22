using UnityEngine;

public class WaveStone : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject prompt;

    WaveManager waveManager;
    bool isHighlighted;

    private void Awake()
    {
        waveManager = FindFirstObjectByType<WaveManager>();
        prompt.SetActive(false);
    }

    private void OnEnable()
    {
        waveManager.WaveStarted += OnWaveChanged;
        waveManager.WaveCompleted += OnWaveChanged;
    }

    private void OnDisable()
    {
        waveManager.WaveStarted -= OnWaveChanged;
        waveManager.WaveCompleted -= OnWaveChanged;
    }

    public void Interact()
    {
        waveManager.StartWave();
    }

    public void SetHighlighted(bool highlighted)
    {
        isHighlighted = highlighted;
        UpdatePrompt();
    }

    private void OnWaveChanged(int wave)
    {
        UpdatePrompt();
    }

    private void UpdatePrompt()
    {
        prompt.SetActive(isHighlighted && !waveManager.IsWaveActive);
    }
}
