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
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;

            AudioListener.volume = PlayerPrefs.GetFloat(VolumeKey, defaultVolume);
        }

        private void Start()
        {
            musicSource.clip = library.Music;
            musicSource.volume = library.MusicVolume;
            musicSource.Play();
        }

        private void OnEnable()
        {
            waveManager.EnemyKilled += OnEnemyKilled;
            waveManager.WaveCompleted += OnWaveCompleted;
            playerHealth.Damaged += OnPlayerDamaged;
            playerShooting.Fired += OnFired;
            playerUpgrades.Purchased += OnPurchased;
            ui.ButtonClicked += OnButtonClicked;
        }

        private void OnDisable()
        {
            waveManager.EnemyKilled -= OnEnemyKilled;
            waveManager.WaveCompleted -= OnWaveCompleted;
            playerHealth.Damaged -= OnPlayerDamaged;
            playerShooting.Fired -= OnFired;
            playerUpgrades.Purchased -= OnPurchased;
            ui.ButtonClicked -= OnButtonClicked;
        }

        public void PlayGameOver()
        {
            Play(library.GameOver);
        }

        private void OnEnemyKilled(Enemy enemy)
        {
            Play(library.EnemyDeath);
        }

        private void OnWaveCompleted(int wave)
        {
            Play(library.WaveComplete);
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

        private void Play(AudioClip clip)
        {
            if (clip != null)
            {
                sfxSource.PlayOneShot(clip);
            }
        }
    }
}
