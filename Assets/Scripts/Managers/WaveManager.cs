using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    enum State { Preparing, InWave }

    [Header("Enemies")]
    [SerializeField] Enemy zombiePrefab;
    [SerializeField] Enemy slimePrefab;
    [SerializeField] int slimeStartWave = 3;
    [SerializeField, Range(0f, 1f)] float slimeChance = 0.3f;

    [Header("Spawning")]
    [SerializeField] Transform[] spawnPoints;

    [Header("Difficulty")]
    [SerializeField] int baseEnemyCount = 5;
    [SerializeField] int enemiesPerWave = 2;
    [SerializeField] float baseSpawnInterval = 2f;
    [SerializeField] float spawnIntervalDecrease = 0.1f;
    [SerializeField] float minSpawnInterval = 0.5f;
    [SerializeField] float healthIncreasePerWave = 0.15f;
    [SerializeField] float speedIncreasePerWave = 0.02f;
    [SerializeField] float maxSpeedMultiplier = 1.5f;
    [SerializeField] int wavesPerBonusDamage = 7;

    [Header("Elite Enemies")]
    [SerializeField] float eliteChancePerWave = 0.03f;
    [SerializeField, Range(0f, 1f)] float maxEliteChance = 0.5f;
    [SerializeField] float eliteHealthMultiplier = 2f;
    [SerializeField] int eliteBonusDamage = 1;
    [SerializeField] int eliteCoinMultiplier = 3;
    [SerializeField] Color eliteTint = new Color(1f, 0.45f, 0.45f);

    State state = State.Preparing;
    int enemiesAlive;
    int enemiesLeftToSpawn;
    Camera mainCamera;
    Health playerHealth;

    public int CurrentWave { get; private set; }
    public bool IsWaveActive => state == State.InWave;

    public event Action<int> WaveStarted;
    public event Action<int> WaveCompleted;

    private void Awake()
    {
        mainCamera = Camera.main;
        playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
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
        enemiesLeftToSpawn = baseEnemyCount + enemiesPerWave * CurrentWave;

        WaveStarted?.Invoke(CurrentWave);
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        float interval = Mathf.Max(minSpawnInterval, baseSpawnInterval - spawnIntervalDecrease * (CurrentWave - 1));
        WaitForSeconds wait = new WaitForSeconds(interval);

        while (enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            yield return wait;
        }
    }

    private void SpawnEnemy()
    {
        Enemy prefab = ChooseEnemyPrefab();
        Vector2 position = ChooseSpawnPoint().position;

        Enemy enemy = Instantiate(prefab, position, Quaternion.identity);
        enemy.ApplyWaveStats(CalculateWaveStats());
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

    private Enemy ChooseEnemyPrefab()
    {
        bool canSpawnSlime = CurrentWave >= slimeStartWave;
        return canSpawnSlime && UnityEngine.Random.value < slimeChance ? slimePrefab : zombiePrefab;
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

    private WaveStats CalculateWaveStats()
    {
        int wavesPassed = CurrentWave - 1;

        WaveStats stats = new WaveStats
        {
            HealthMultiplier = 1f + healthIncreasePerWave * wavesPassed,
            SpeedMultiplier = Mathf.Min(1f + speedIncreasePerWave * wavesPassed, maxSpeedMultiplier),
            BonusDamage = wavesPassed / wavesPerBonusDamage,
            CoinMultiplier = 1,
            Tint = Color.white
        };

        if (RollElite(wavesPassed))
        {
            stats.HealthMultiplier *= eliteHealthMultiplier;
            stats.BonusDamage += eliteBonusDamage;
            stats.CoinMultiplier = eliteCoinMultiplier;
            stats.Tint = eliteTint;
        }

        return stats;
    }

    private bool RollElite(int wavesPassed)
    {
        float eliteChance = Mathf.Min(eliteChancePerWave * wavesPassed, maxEliteChance);
        return UnityEngine.Random.value < eliteChance;
    }
}
