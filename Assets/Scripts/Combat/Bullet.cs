using UnityEngine;

namespace Tilemania.Combat
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] float bulletSpeed = 20f;
        [SerializeField] float bulletLifeTime = 1f;

        int damage;
        bool hasHit;

        public void Launch(float direction, int damage)
        {
            this.damage = damage;
            GetComponent<Rigidbody2D>().linearVelocityX = direction * bulletSpeed;
            Destroy(gameObject, bulletLifeTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (hasHit) return;
            hasHit = true;

            if (other.TryGetComponent(out Health health))
            {
                health.TakeDamage(damage, transform.position);
            }
            Destroy(gameObject);
        }
    }
}
