using System;
using UnityEngine;
using Tilemania.Player;

namespace Tilemania.Coin
{
    public class CoinPickup : MonoBehaviour
    {
        [SerializeField] AudioClip coinPickupSFX;
        [SerializeField] float attractSpeed = 12f;

        Rigidbody2D coinRB;
        Collider2D coinCollider;
        Transform attractTarget;
        bool wasCollected;

        public event Action<CoinPickup> Collected;

        private void Awake()
        {
            coinRB = GetComponent<Rigidbody2D>();
            coinCollider = GetComponent<Collider2D>();
        }

        private void FixedUpdate()
        {
            if (attractTarget == null) return;

            Vector2 toTarget = attractTarget.position - transform.position;
            coinRB.linearVelocity = toTarget.normalized * attractSpeed;
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
            if (wasCollected || !other.TryGetComponent(out PlayerWallet wallet)) return;
            wasCollected = true;

            wallet.Add(1);
            AudioSource.PlayClipAtPoint(coinPickupSFX, transform.position);
            Collected?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
