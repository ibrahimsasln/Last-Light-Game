using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Tilemania.Combat;

namespace Tilemania.Player
{
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField] Bullet bulletPrefab;
        [SerializeField] float fireCooldown = 0.2f;
        [SerializeField] float minFireCooldown = 0.05f;
        [SerializeField] int bulletDamage = 1;
        [SerializeField] int projectileCount = 1;
        [SerializeField] float spreadAngle = 10f;
        [SerializeField] float muzzleOffset = 0.3f;

        Health health;
        PlayerAim aim;
        float nextFireTime;

        public event Action Fired;

        private void Awake()
        {
            health = GetComponent<Health>();
            aim = GetComponent<PlayerAim>();
        }

        private void OnAttack(InputValue value)
        {
            if (health.IsDead) return;
            if (Time.time < nextFireTime) return;

            nextFireTime = Time.time + fireCooldown;

            Vector2 spawnPosition = aim.GunPosition + aim.AimDirection * muzzleOffset;
            for (int i = 0; i < projectileCount; i++)
            {
                float angle = (i - (projectileCount - 1) / 2f) * spreadAngle;
                Vector2 direction = Quaternion.Euler(0f, 0f, angle) * aim.AimDirection;

                Bullet bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
                bullet.Launch(direction, bulletDamage);
            }

            Fired?.Invoke();
        }

        public void MultiplyCooldown(float multiplier)
        {
            fireCooldown = Mathf.Max(minFireCooldown, fireCooldown * multiplier);
        }

        public void AddDamage(int amount)
        {
            bulletDamage += amount;
        }

        public void AddProjectiles(int amount)
        {
            projectileCount += amount;
        }
    }
}
