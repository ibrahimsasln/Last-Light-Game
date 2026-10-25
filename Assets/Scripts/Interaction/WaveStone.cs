using UnityEngine;
using Tilemania.Managers;

namespace Tilemania.Interaction
{
    public class WaveStone : MonoBehaviour, IInteractable
    {
        [SerializeField] WaveManager waveManager;
        [SerializeField] GameObject prompt;

        bool isHighlighted;

        private void Awake()
        {
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
}
