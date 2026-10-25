using UnityEngine;

namespace Tilemania.Interaction
{
    public class WaveStone : BetweenWavesInteractable
    {
        [SerializeField] GameObject glow;

        protected override void Awake()
        {
            base.Awake();
            SetGlow(true);
        }

        protected override void OnInteract()
        {
            WaveManager.StartWave();
        }

        protected override void OnWaveStateChanged(bool isWaveActive)
        {
            SetGlow(!isWaveActive);
        }

        private void SetGlow(bool isOn)
        {
            if (glow != null)
            {
                glow.SetActive(isOn);
            }
        }
    }
}
