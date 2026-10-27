using System.Collections;
using UnityEngine;
using Tilemania.Combat;
using Tilemania.Enemies;
using Tilemania.Managers;
using Tilemania.Player;
using Tilemania.UI;
using Tilemania.Upgrades;

namespace Tilemania.Audio
{
    public class AudioManager : MonoBehaviour
    {
        const string VolumeKey = "Volume";

        [SerializeField] AudioLibrary library;
        [SerializeField] float defaultVolume = 0.8f;

        [Header("Event Sources")]
        [SerializeField] WaveManager waveManager;
        [SerializeField] Health playerHealth;
        [SerializeField] PlayerShooting playerShooting;
        [SerializeField] PlayerUpgrades playerUpgrades;

        UIManager ui;
        AudioSource sfxSource;
        AudioSource musicSource;
        AudioSource fadingMusicSource;
        Coroutine musicFade;

        public float Volume
        {
            get => AudioListener.volume;
            set
            {
                AudioListener.volume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat(VolumeKey, AudioListener.volume);
            }
        }

        private void Awake()
        {
            ui = GetComponent<UIManager>();
            sfxSource = gameObject.AddComponent<AudioSource>();
            musicSource = CreateMusicSource();
            fadingMusicSource = CreateMusicSource();

            AudioListener.volume = PlayerPrefs.GetFloat(VolumeKey, defaultVolume);
        }

        private void Start()
        {
            musicSource.clip = library.PreparationMusic;
            musicSource.volume = library.MusicVolume;
            musicSource.Play();
        }

        private void OnEnable()
        {
            waveManager.EnemyKilled += OnEnemyKilled;
            waveManager.WaveStarted += OnWaveStarted;
            waveManager.WaveCompleted += OnWaveCompleted;
            playerHealth.Damaged += OnPlayerDamaged;
            playerShooting.Fired += OnFired;
            playerUpgrades.Purchased += OnPurchased;
            ui.ButtonClicked += OnButtonClicked;
        }

        private void OnDisable()
        {
            waveManager.EnemyKilled -= OnEnemyKilled;
            waveManager.WaveStarted -= OnWaveStarted;
            waveManager.WaveCompleted -= OnWaveCompleted;
            playerHealth.Damaged -= OnPlayerDamaged;
            playerShooting.Fired -= OnFired;
            playerUpgrades.Purchased -= OnPurchased;
            ui.ButtonClicked -= OnButtonClicked;
        }

        public void SaveSettings()
        {
            PlayerPrefs.Save();
        }

        public void PlayGameOver()
        {
            Play(library.GameOver);
        }

        private void OnEnemyKilled(Enemy enemy)
        {
            Play(library.EnemyDeath);
        }

        private void OnWaveStarted(int wave)
        {
            CrossfadeMusic(library.WaveMusic, library.WaveMusicPitch);
        }

        private void OnWaveCompleted(int wave)
        {
            Play(library.WaveComplete);
            CrossfadeMusic(library.PreparationMusic, 1f);
        }

        private void OnPlayerDamaged(Vector2 hitFrom)
        {
            Play(library.PlayerHurt);
        }

        private void OnFired()
        {
            Play(library.Shoot);
        }

        private void OnPurchased(UpgradeDefinition upgrade)
        {
            Play(library.Purchase);
        }

        private void OnButtonClicked()
        {
            Play(library.Click);
        }

        private void CrossfadeMusic(AudioClip clip, float pitch)
        {
            if (musicFade != null)
            {
                StopCoroutine(musicFade);
            }
            musicFade = StartCoroutine(Crossfade(clip, pitch));
        }

        private IEnumerator Crossfade(AudioClip clip, float pitch)
        {
            (musicSource, fadingMusicSource) = (fadingMusicSource, musicSource);

            musicSource.clip = clip;
            musicSource.pitch = pitch;
            musicSource.volume = 0f;
            musicSource.Play();

            float fadingStartVolume = fadingMusicSource.volume;
            float duration = library.MusicFadeDuration;

            for (float elapsed = 0f; elapsed < duration; elapsed += Time.unscaledDeltaTime)
            {
                float progress = elapsed / duration;
                musicSource.volume = progress * library.MusicVolume;
                fadingMusicSource.volume = (1f - progress) * fadingStartVolume;
                yield return null;
            }

            musicSource.volume = library.MusicVolume;
            fadingMusicSource.Stop();
            musicFade = null;
        }

        private AudioSource CreateMusicSource()
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.loop = true;
            return source;
        }

        private void Play(AudioClip clip)
        {
            if (clip != null)
            {
                sfxSource.PlayOneShot(clip);
            }
        }
    }
}
