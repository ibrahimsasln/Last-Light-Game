using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using LastLight.Combat;

namespace LastLight.Player
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

        [Header("Pool")]
        [SerializeField] int poolCapacity = 20;
        [SerializeField] int maxPoolSize = 100;

        Health health;
        PlayerAim aim;
        IObjectPool<Bullet> bulletPool;
        float nextFireTime;

        public event Action Fired;

        public bool IsAtMinCooldown => fireCooldown <= minFireCooldown;

        private void Awake()
        {
            health = GetComponent<Health>();
            aim = GetComponent<PlayerAim>();
            bulletPool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet,
                defaultCapacity: poolCapacity, maxSize: maxPoolSize);
        }

        private Bullet CreateBullet()
        {
            Bullet bullet = Instantiate(bulletPrefab);
            bullet.SetPool(bulletPool);
            return bullet;
        }

        private void OnGetBullet(Bullet bullet)
        {
            bullet.gameObject.SetActive(true);
        }

        private void OnReleaseBullet(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
        }

        private void OnDestroyBullet(Bullet bullet)
        {
            Destroy(bullet.gameObject);
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

                bulletPool.Get().Launch(spawnPosition, direction, bulletDamage);
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
