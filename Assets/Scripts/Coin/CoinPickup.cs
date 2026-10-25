using UnityEngine;
using Tilemania.Core;
using Tilemania.Managers;

namespace Tilemania.Coin
{
    public class CoinPickup : MonoBehaviour
    {
        [SerializeField] AudioClip coinPickupSFX;

        bool wasCollected;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (wasCollected || !other.gameObject.CompareTag(Tags.Player)) return;
            wasCollected = true;

            GameManager.Instance.AddCoins(1);
            AudioSource.PlayClipAtPoint(coinPickupSFX, transform.position);
            Destroy(gameObject);
        }
    }
}
