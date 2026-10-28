using System;
using UnityEngine;

namespace Tilemania.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth = 3;
        [SerializeField] float invulnerabilityDuration = 0f;

        float invulnerableUntil;

        public int CurrentHealth { get; private set; }
        public int MaxHealth => maxHealth;
        public bool IsDead { get; private set; }

        public event Action<int, int> HealthChanged;
        public event Action<Vector2> Damaged;
        public event Action Died;

        public bool IsFullHealth => CurrentHealth >= maxHealth;

        bool IsInvulnerable => Time.time < invulnerableUntil;

        private void Awake()
        {
            CurrentHealth = maxHealth;
        }

        public void ResetHealth(int newMaxHealth)
        {
            maxHealth = Mathf.Max(1, newMaxHealth);
            CurrentHealth = maxHealth;
            IsDead = false;
            invulnerableUntil = 0f;
            HealthChanged?.Invoke(CurrentHealth, maxHealth);
        }

        public void IncreaseMaxHealth(int amount)
        {
            maxHealth += amount;
            CurrentHealth += amount;
            HealthChanged?.Invoke(CurrentHealth, maxHealth);
        }

        public void HealToFull()
        {
            if (IsDead) return;

            CurrentHealth = maxHealth;
            HealthChanged?.Invoke(CurrentHealth, maxHealth);
        }

        public void MakeInvulnerable(float duration)
        {
            invulnerableUntil = Mathf.Max(invulnerableUntil, Time.time + duration);
        }

        public void Kill()
        {
            if (IsDead) return;

            CurrentHealth = 0;
            HealthChanged?.Invoke(CurrentHealth, maxHealth);
            IsDead = true;
            Died?.Invoke();
        }

        public void TakeDamage(int amount, Vector2 hitFrom)
        {
            if (IsDead || IsInvulnerable) return;

            CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
            invulnerableUntil = Time.time + invulnerabilityDuration;
            HealthChanged?.Invoke(CurrentHealth, maxHealth);

            if (CurrentHealth == 0)
            {
                IsDead = true;
                Died?.Invoke();
            }
            else
            {
                Damaged?.Invoke(hitFrom);
            }
        }
    }
}
