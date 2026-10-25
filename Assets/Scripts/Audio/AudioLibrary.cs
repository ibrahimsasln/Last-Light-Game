using UnityEngine;

namespace Tilemania.Audio
{
    [CreateAssetMenu(fileName = "AudioLibrary", menuName = "Tilemania/Audio Library")]
    public class AudioLibrary : ScriptableObject
    {
        [Header("Music")]
        [SerializeField] AudioClip music;
        [SerializeField, Range(0f, 1f)] float musicVolume = 0.35f;

        [Header("Sound Effects")]
        [SerializeField] AudioClip shoot;
        [SerializeField] AudioClip playerHurt;
        [SerializeField] AudioClip enemyDeath;
        [SerializeField] AudioClip waveComplete;
        [SerializeField] AudioClip purchase;
        [SerializeField] AudioClip click;
        [SerializeField] AudioClip gameOver;

        public AudioClip Music => music;
        public float MusicVolume => musicVolume;
        public AudioClip Shoot => shoot;
        public AudioClip PlayerHurt => playerHurt;
        public AudioClip EnemyDeath => enemyDeath;
        public AudioClip WaveComplete => waveComplete;
        public AudioClip Purchase => purchase;
        public AudioClip Click => click;
        public AudioClip GameOver => gameOver;
    }
}
