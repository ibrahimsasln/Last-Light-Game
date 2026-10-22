using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int contactDamage = 1;
    [SerializeField] int coinDropCount = 3;

    Health health;
    SpriteRenderer spriteRenderer;
    IEnemyMovement movement;
    CoinDropper coinDropper;

    public event Action<Enemy> Died;

    private void Awake()
    {
        health = GetComponent<Health>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<IEnemyMovement>();
        coinDropper = FindFirstObjectByType<CoinDropper>();
    }

    private void OnEnable()
    {
        health.Died += OnDied;
    }

    private void OnDisable()
    {
        health.Died -= OnDied;
    }

    public void ApplyWaveStats(WaveStats stats)
    {
        health.SetMaxHealth(Mathf.RoundToInt(health.MaxHealth * stats.HealthMultiplier));
        contactDamage += stats.BonusDamage;
        coinDropCount *= stats.CoinMultiplier;
        spriteRenderer.color = stats.Tint;
        movement.SetSpeedMultiplier(stats.SpeedMultiplier);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (health.IsDead || !collision.gameObject.CompareTag("Player")) return;

        if (collision.gameObject.TryGetComponent(out Health playerHealth))
        {
            playerHealth.TakeDamage(contactDamage, transform.position);
        }
    }

    private void OnDied()
    {
        Died?.Invoke(this);
        coinDropper.DropCoins(transform.position, coinDropCount);
        Destroy(gameObject);
    }
}
