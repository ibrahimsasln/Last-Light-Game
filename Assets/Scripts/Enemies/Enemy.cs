using System;
using UnityEngine;
using Tilemania.Coin;
using Tilemania.Combat;
using Tilemania.Core;

namespace Tilemania.Enemies
{
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
        }

        private void OnEnable()
        {
            health.Died += OnDied;
        }

        private void OnDisable()
        {
            health.Died -= OnDied;
        }

        public void Initialize(Health target, CoinDropper coinDropper, WaveStats stats)
        {
            this.coinDropper = coinDropper;

            health.SetMaxHealth(Mathf.RoundToInt(health.MaxHealth * stats.HealthMultiplier));
            contactDamage += stats.BonusDamage;
            coinDropCount *= stats.CoinMultiplier;
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
            Destroy(gameObject);
        }
    }
}
