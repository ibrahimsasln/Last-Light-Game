using UnityEngine;
using UnityEngine.InputSystem;
using Tilemania.Combat;

namespace Tilemania.Player
{
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField] Bullet bulletPrefab;
        [SerializeField] Transform gun;
        [SerializeField] float fireCooldown = 0.2f;
        [SerializeField] int bulletDamage = 1;

        Health health;
        float nextFireTime;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void OnAttack(InputValue value)
        {
            if (health.IsDead) return;
            if (Time.time < nextFireTime) return;

            nextFireTime = Time.time + fireCooldown;

            Bullet bullet = Instantiate(bulletPrefab, gun.position, Quaternion.identity);
            bullet.Launch(Mathf.Sign(transform.localScale.x), bulletDamage);
        }
    }
}
