using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tilemania.Coin;
using Tilemania.Combat;
using Tilemania.Enemies;

namespace Tilemania.Managers
{
    public class WaveManager : MonoBehaviour
    {
        enum State { Preparing, InWave }

        [Header("References")]
        [SerializeField] WaveSettings settings;
        [SerializeField] Health playerHealth;
        [SerializeField] CoinDropper coinDropper;

        [Header("Spawning")]
        [SerializeField] Transform[] spawnPoints;

        State state = State.Preparing;
        int enemiesAlive;
        int enemiesLeftToSpawn;
        Camera mainCamera;

        public int CurrentWave { get; private set; }
        public bool IsWaveActive => state == State.InWave;

        public event Action<int> WaveStarted;
        public event Action<int> WaveCompleted;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            playerHealth.Died += StopWave;
        }

        private void OnDisable()
        {
            playerHealth.Died -= StopWave;
        }

        public void StartWave()
        {
            if (state == State.InWave || playerHealth.IsDead) return;

            CurrentWave++;
            state = State.InWave;
            enemiesAlive = 0;
            enemiesLeftToSpawn = settings.GetEnemyCount(CurrentWave);

            WaveStarted?.Invoke(CurrentWave);
            StartCoroutine(SpawnEnemies());
        }

        private IEnumerator SpawnEnemies()
        {
            WaitForSeconds wait = new WaitForSeconds(settings.GetSpawnInterval(CurrentWave));

            while (enemiesLeftToSpawn > 0)
            {
                SpawnEnemy();
                enemiesLeftToSpawn--;
                yield return wait;
            }
        }

        private void SpawnEnemy()
        {
            Enemy prefab = settings.ChooseEnemy(CurrentWave, UnityEngine.Random.value);
            bool isElite = UnityEngine.Random.value < settings.GetEliteChance(CurrentWave);
            Vector2 position = ChooseSpawnPoint().position;

            Enemy enemy = Instantiate(prefab, position, Quaternion.identity);
            enemy.Initialize(playerHealth, coinDropper, settings.CreateStats(CurrentWave, isElite));
            enemy.Died += OnEnemyDied;
            enemiesAlive++;
        }

        private void OnEnemyDied(Enemy enemy)
        {
            enemy.Died -= OnEnemyDied;
            enemiesAlive--;

            if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
            {
                CompleteWave();
            }
        }

        private void CompleteWave()
        {
            state = State.Preparing;
            WaveCompleted?.Invoke(CurrentWave);
        }

        private void StopWave()
        {
            StopAllCoroutines();
        }

        private Transform ChooseSpawnPoint()
        {
            List<Transform> hiddenPoints = new List<Transform>();
            foreach (Transform point in spawnPoints)
            {
                if (!IsOnScreen(point.position))
                {
                    hiddenPoints.Add(point);
                }
            }

            List<Transform> candidates = hiddenPoints.Count > 0 ? hiddenPoints : new List<Transform>(spawnPoints);
            return candidates[UnityEngine.Random.Range(0, candidates.Count)];
        }

        private bool IsOnScreen(Vector2 position)
        {
            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(position);
            return viewportPoint.x >= 0f && viewportPoint.x <= 1f && viewportPoint.y >= 0f && viewportPoint.y <= 1f;
        }
    }
}
