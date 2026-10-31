using System.Collections.Generic;
using UnityEngine;
using LastLight.Enemies;

namespace LastLight.Managers
{
    [CreateAssetMenu(fileName = "WaveSettings", menuName = "Last Light/Wave Settings")]
    public class WaveSettings : ScriptableObject
    {
        [Header("Enemies")]
        [SerializeField] List<EnemySpawnEntry> enemies = new List<EnemySpawnEntry>();

        [Header("Wave Size")]
        [SerializeField] int baseEnemyCount = 5;
        [SerializeField] int enemiesPerWave = 2;

        [Header("Spawn Rate")]
        [SerializeField] float baseSpawnInterval = 2f;
        [SerializeField] float spawnIntervalDecrease = 0.1f;
        [SerializeField] float minSpawnInterval = 0.5f;

        [Header("Scaling")]
        [SerializeField] float healthIncreasePerWave = 0.15f;
        [SerializeField] float speedIncreasePerWave = 0.02f;
        [Tooltip("0 means enemies keep getting faster forever.")]
        [SerializeField, Min(0f)] float maxSpeedMultiplier;
        [SerializeField] int wavesPerBonusDamage = 7;

        [Header("Elite Enemies")]
        [SerializeField] float eliteChancePerWave = 0.03f;
        [SerializeField, Range(0f, 1f)] float maxEliteChance = 0.5f;
        [SerializeField] float eliteHealthMultiplier = 2f;
        [SerializeField] int eliteBonusDamage = 1;
        [SerializeField] int eliteCoinMultiplier = 3;
        [SerializeField] Color eliteTint = new Color(1f, 0.45f, 0.45f);

        public int GetEnemyCount(int wave)
        {
            return baseEnemyCount + enemiesPerWave * wave;
        }

        public float GetSpawnInterval(int wave)
        {
            return Mathf.Max(minSpawnInterval, baseSpawnInterval - spawnIntervalDecrease * (wave - 1));
        }

        public float GetEliteChance(int wave)
        {
            return Mathf.Min(eliteChancePerWave * (wave - 1), maxEliteChance);
        }

        public Enemy ChooseEnemy(int wave, float randomValue)
        {
            float totalWeight = 0f;
            foreach (EnemySpawnEntry entry in enemies)
            {
                if (entry.IsAvailable(wave))
                {
                    totalWeight += entry.Weight;
                }
            }

            float pick = randomValue * totalWeight;
            foreach (EnemySpawnEntry entry in enemies)
            {
                if (!entry.IsAvailable(wave)) continue;

                if (pick < entry.Weight)
                {
                    return entry.Prefab;
                }
                pick -= entry.Weight;
            }

            return enemies[0].Prefab;
        }

        private float GetSpeedMultiplier(int wavesPassed)
        {
            float multiplier = 1f + speedIncreasePerWave * wavesPassed;
            return maxSpeedMultiplier > 0f ? Mathf.Min(multiplier, maxSpeedMultiplier) : multiplier;
        }

        public WaveStats CreateStats(int wave, bool isElite)
        {
            int wavesPassed = wave - 1;

            WaveStats stats = new WaveStats
            {
                HealthMultiplier = 1f + healthIncreasePerWave * wavesPassed,
                SpeedMultiplier = GetSpeedMultiplier(wavesPassed),
                BonusDamage = wavesPassed / wavesPerBonusDamage,
                CoinMultiplier = 1,
                Tint = Color.white
            };

            if (isElite)
            {
                stats.HealthMultiplier *= eliteHealthMultiplier;
                stats.BonusDamage += eliteBonusDamage;
                stats.CoinMultiplier = eliteCoinMultiplier;
                stats.Tint = eliteTint;
            }

            return stats;
        }
    }
}
