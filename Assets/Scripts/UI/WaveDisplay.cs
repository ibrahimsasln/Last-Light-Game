using TMPro;
using UnityEngine;
using Tilemania.Managers;

namespace Tilemania.UI
{
    public class WaveDisplay : MonoBehaviour
    {
        [SerializeField] WaveManager waveManager;

        TextMeshProUGUI waveText;

        private void Awake()
        {
            waveText = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            waveManager.WaveStarted += ShowWaveStarted;
            waveManager.WaveCompleted += ShowWaveCompleted;
        }

        private void OnDisable()
        {
            waveManager.WaveStarted -= ShowWaveStarted;
            waveManager.WaveCompleted -= ShowWaveCompleted;
        }

        private void Start()
        {
            waveText.text = "Press E at the stone to start";
        }

        private void ShowWaveStarted(int wave)
        {
            waveText.text = $"Wave {wave}";
        }

        private void ShowWaveCompleted(int wave)
        {
            waveText.text = $"Wave {wave} complete!";
        }
    }
}
