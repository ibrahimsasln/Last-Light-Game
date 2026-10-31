using System;
using UnityEngine;
using UnityEngine.Pool;
using LastLight.Coin;
using LastLight.Combat;
using LastLight.Core;

namespace LastLight.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] int contactDamage = 1;
        [SerializeField] int coinDropCount = 3;

        Health health;
        SpriteRenderer spriteRenderer;
        IEnemyMovement movement;
        CoinDropper coinDropper;
        IObjectPool<Enemy> pool;

        int baseMaxHealth;
        int baseContactDamage;
        int baseCoinDropCount;

        public event Action<Enemy> Died;

        private void Awake()
        {
            health = GetComponent<Health>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            movement = GetComponent<IEnemyMovement>();

            baseMaxHealth = health.MaxHealth;
            baseContactDamage = contactDamage;
            baseCoinDropCount = coinDropCount;
        }

        private void OnEnable()
        {
            health.Died += OnDied;
        }

        private void OnDisable()
        {
            health.Died -= OnDied;
        }

        public void SetPool(IObjectPool<Enemy> pool)
        {
            this.pool = pool;
        }

        public void Initialize(Health target, CoinDropper coinDropper, WaveStats stats)
        {
            this.coinDropper = coinDropper;

            health.ResetHealth(Mathf.RoundToInt(baseMaxHealth * stats.HealthMultiplier));
            contactDamage = baseContactDamage + stats.BonusDamage;
            coinDropCount = baseCoinDropCount * stats.CoinMultiplier;
            spriteRenderer.color = stats.Tint;
            movement.Initialize(target, stats.SpeedMultiplier);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            TryDamage(collision.gameObject);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            TryDamage(other.gameObject);
        }

        private void TryDamage(GameObject other)
        {
            if (health.IsDead || !other.CompareTag(Tags.Player)) return;

            if (other.TryGetComponent(out Health playerHealth))
            {
                playerHealth.TakeDamage(contactDamage, transform.position);
            }
        }

        private void OnDied()
        {
            coinDropper.DropCoins(transform.position, coinDropCount);
            Died?.Invoke(this);
            pool.Release(this);
        }
    }
}
