using System.Collections.Generic;
using UnityEngine;
using Tilemania.Managers;

namespace Tilemania.Coin
{
    public class CoinDropper : MonoBehaviour
    {
        [SerializeField] GameObject coinPrefab;
        [SerializeField] float minDropForce = 3f;
        [SerializeField] float maxDropForce = 6f;

        [Header("Collect On Wave End")]
        [SerializeField] WaveManager waveManager;
        [SerializeField] Transform player;

        readonly List<CoinPickup> activeCoins = new List<CoinPickup>();

        private void OnEnable()
        {
            waveManager.WaveCompleted += OnWaveCompleted;
        }

        private void OnDisable()
        {
            waveManager.WaveCompleted -= OnWaveCompleted;
        }

        public void DropCoins(Vector2 position, int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject coin = Instantiate(coinPrefab, position, Quaternion.identity);

                Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
                float force = Random.Range(minDropForce, maxDropForce);
                coin.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);

                CoinPickup pickup = coin.GetComponent<CoinPickup>();
                pickup.Collected += OnCoinCollected;
                activeCoins.Add(pickup);
            }
        }

        private void OnCoinCollected(CoinPickup coin)
        {
            coin.Collected -= OnCoinCollected;
            activeCoins.Remove(coin);
        }

        private void OnWaveCompleted(int wave)
        {
            activeCoins.RemoveAll(coin => coin == null);

            foreach (CoinPickup coin in activeCoins)
            {
                coin.AttractTo(player);
            }
        }
    }
}
