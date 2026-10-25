using UnityEngine;
using Tilemania.Player;

namespace Tilemania.Coin
{
    public class CoinPickup : MonoBehaviour
    {
        [SerializeField] AudioClip coinPickupSFX;

        bool wasCollected;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (wasCollected || !other.gameObject.TryGetComponent(out PlayerWallet wallet)) return;
            wasCollected = true;

            wallet.Add(1);
            AudioSource.PlayClipAtPoint(coinPickupSFX, transform.position);
            Destroy(gameObject);
        }
    }
}
