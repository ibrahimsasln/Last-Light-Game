using UnityEngine;

namespace LastLight.Audio
{
    [CreateAssetMenu(fileName = "AudioLibrary", menuName = "Last Light/Audio Library")]
    public class AudioLibrary : ScriptableObject
    {
        [Header("Music")]
        [SerializeField] AudioClip preparationMusic;
        [SerializeField] AudioClip waveMusic;
        [SerializeField, Range(0.5f, 1.5f)] float waveMusicPitch = 1f;
        [SerializeField, Range(0f, 1f)] float musicVolume = 0.35f;
        [SerializeField] float musicFadeDuration = 1.5f;

        [Header("Sound Effects")]
        [SerializeField] AudioClip shoot;
        [SerializeField] AudioClip playerHurt;
        [SerializeField] AudioClip enemyDeath;
        [SerializeField] AudioClip waveComplete;
        [SerializeField] AudioClip purchase;
        [SerializeField] AudioClip click;
        [SerializeField] AudioClip gameOver;

        public AudioClip PreparationMusic => preparationMusic;
        public AudioClip WaveMusic => waveMusic;
        public float WaveMusicPitch => waveMusicPitch;
        public float MusicVolume => musicVolume;
        public float MusicFadeDuration => musicFadeDuration;
        public AudioClip Shoot => shoot;
        public AudioClip PlayerHurt => playerHurt;
        public AudioClip EnemyDeath => enemyDeath;
        public AudioClip WaveComplete => waveComplete;
        public AudioClip Purchase => purchase;
        public AudioClip Click => click;
        public AudioClip GameOver => gameOver;
    }
}
