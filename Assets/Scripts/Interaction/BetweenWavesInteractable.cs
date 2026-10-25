using UnityEngine;
using Tilemania.Managers;

namespace Tilemania.Interaction
{
    public abstract class BetweenWavesInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] WaveManager waveManager;
        [SerializeField] GameObject prompt;

        bool isHighlighted;

        protected WaveManager WaveManager => waveManager;

        protected virtual void Awake()
        {
            prompt.SetActive(false);
        }

        protected virtual void OnEnable()
        {
            waveManager.WaveStarted += HandleWaveChanged;
            waveManager.WaveCompleted += HandleWaveChanged;
        }

        protected virtual void OnDisable()
        {
            waveManager.WaveStarted -= HandleWaveChanged;
            waveManager.WaveCompleted -= HandleWaveChanged;
        }

        public void Interact()
        {
            if (waveManager.IsWaveActive) return;

            OnInteract();
        }

        public void SetHighlighted(bool highlighted)
        {
            isHighlighted = highlighted;
            UpdatePrompt();
        }

        protected abstract void OnInteract();

        protected virtual void OnWaveStateChanged(bool isWaveActive)
        {
        }

        private void HandleWaveChanged(int wave)
        {
            UpdatePrompt();
            OnWaveStateChanged(waveManager.IsWaveActive);
        }

        private void UpdatePrompt()
        {
            prompt.SetActive(isHighlighted && !waveManager.IsWaveActive);
        }
    }
}
