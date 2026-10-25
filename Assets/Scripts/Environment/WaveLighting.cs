using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Tilemania.Managers;

namespace Tilemania.Environment
{
    public class WaveLighting : MonoBehaviour
    {
        [SerializeField] WaveManager waveManager;
        [SerializeField] Light2D globalLight;
        [SerializeField] float idleIntensity = 1f;
        [SerializeField] float waveIntensity = 0.45f;
        [SerializeField] float transitionDuration = 1.5f;

        Coroutine transition;

        private void OnEnable()
        {
            waveManager.WaveStarted += OnWaveStarted;
            waveManager.WaveCompleted += OnWaveCompleted;
        }

        private void OnDisable()
        {
            waveManager.WaveStarted -= OnWaveStarted;
            waveManager.WaveCompleted -= OnWaveCompleted;
        }

        private void OnWaveStarted(int wave)
        {
            FadeTo(waveIntensity);
        }

        private void OnWaveCompleted(int wave)
        {
            FadeTo(idleIntensity);
        }

        private void FadeTo(float targetIntensity)
        {
            if (transition != null)
            {
                StopCoroutine(transition);
            }
            transition = StartCoroutine(Fade(targetIntensity));
        }

        private IEnumerator Fade(float targetIntensity)
        {
            float startIntensity = globalLight.intensity;

            for (float elapsed = 0f; elapsed < transitionDuration; elapsed += Time.deltaTime)
            {
                globalLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / transitionDuration);
                yield return null;
            }

            globalLight.intensity = targetIntensity;
            transition = null;
        }
    }
}
