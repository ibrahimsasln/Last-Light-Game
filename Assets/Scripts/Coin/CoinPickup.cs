using UnityEngine;
using UnityEngine.Pool;
using LastLight.Core;
using LastLight.Player;

namespace LastLight.Coin
{
    public class CoinPickup : MonoBehaviour, IDespawnable
    {
        [SerializeField] AudioClip coinPickupSFX;
        [SerializeField] float attractSpeed = 12f;

        Rigidbody2D coinRB;
        Collider2D coinCollider;
        IObjectPool<CoinPickup> pool;
        Transform attractTarget;
        float defaultGravityScale;
        bool isDespawned;

        private void Awake()
        {
            coinRB = GetComponent<Rigidbody2D>();
            coinCollider = GetComponent<Collider2D>();
            defaultGravityScale = coinRB.gravityScale;
        }

        private void FixedUpdate()
        {
            if (attractTarget == null) return;

            Vector2 toTarget = attractTarget.position - transform.position;
            coinRB.linearVelocity = toTarget.normalized * attractSpeed;
        }

        public void SetPool(IObjectPool<CoinPickup> pool)
        {
            this.pool = pool;
        }

        public void Drop(Vector2 position, Vector2 impulse)
        {
            isDespawned = false;
            attractTarget = null;
            coinRB.gravityScale = defaultGravityScale;
            coinCollider.isTrigger = false;

            transform.position = position;
            coinRB.linearVelocity = Vector2.zero;
            coinRB.AddForce(impulse, ForceMode2D.Impulse);
        }

        public void AttractTo(Transform target)
        {
            attractTarget = target;
            coinRB.gravityScale = 0f;
            coinCollider.isTrigger = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            TryCollect(collision.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TryCollect(other.gameObject);
        }

        private void TryCollect(GameObject other)
        {
            if (isDespawned || !other.TryGetComponent(out PlayerWallet wallet)) return;

            wallet.Add(1);
            AudioSource.PlayClipAtPoint(coinPickupSFX, transform.position);
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
