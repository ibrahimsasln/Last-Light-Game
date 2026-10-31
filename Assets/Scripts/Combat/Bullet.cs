using UnityEngine;
using UnityEngine.Pool;
using LastLight.Core;

namespace LastLight.Combat
{
    public class Bullet : MonoBehaviour, IDespawnable
    {
        [SerializeField] float bulletSpeed = 20f;
        [SerializeField] float bulletLifeTime = 1f;

        Rigidbody2D bulletRB;
        IObjectPool<Bullet> pool;
        int damage;
        float despawnTime;
        bool isDespawned;

        private void Awake()
        {
            bulletRB = GetComponent<Rigidbody2D>();
        }

        public void SetPool(IObjectPool<Bullet> pool)
        {
            this.pool = pool;
        }

        public void Launch(Vector2 position, Vector2 direction, int damage)
        {
            this.damage = damage;
            isDespawned = false;
            despawnTime = Time.time + bulletLifeTime;

            transform.position = position;
            transform.right = direction;
            bulletRB.linearVelocity = direction * bulletSpeed;
        }

        private void Update()
        {
            if (Time.time >= despawnTime)
            {
                Despawn();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isDespawned) return;

            if (other.TryGetComponent(out Health health))
            {
                health.TakeDamage(damage, transform.position);
            }
            Despawn();
        }

        public void Despawn()
        {
            if (isDespawned) return;
            isDespawned = true;

            pool.Release(this);
        }
    }
}
