using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Tilemania.Managers;

namespace Tilemania.Coin
{
    public class CoinDropper : MonoBehaviour
    {
        [SerializeField] CoinPickup coinPrefab;
        [SerializeField] float minDropForce = 3f;
        [SerializeField] float maxDropForce = 6f;

        [Header("Collect On Wave End")]
        [SerializeField] WaveManager waveManager;
        [SerializeField] Transform player;

        [Header("Pool")]
        [SerializeField] int poolCapacity = 30;
        [SerializeField] int maxPoolSize = 200;

        readonly List<CoinPickup> activeCoins = new List<CoinPickup>();
        IObjectPool<CoinPickup> coinPool;

        private void Awake()
        {
            coinPool = new ObjectPool<CoinPickup>(CreateCoin, OnGetCoin, OnReleaseCoin, OnDestroyCoin,
                defaultCapacity: poolCapacity, maxSize: maxPoolSize);
        }

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
                Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
                float force = Random.Range(minDropForce, maxDropForce);

                coinPool.Get().Drop(position, direction * force);
            }
        }

        private CoinPickup CreateCoin()
        {
            CoinPickup coin = Instantiate(coinPrefab);
            coin.SetPool(coinPool);
            return coin;
        }

        private void OnGetCoin(CoinPickup coin)
        {
            coin.gameObject.SetActive(true);
            activeCoins.Add(coin);
        }

        private void OnReleaseCoin(CoinPickup coin)
        {
            coin.gameObject.SetActive(false);
            activeCoins.Remove(coin);
        }

        private void OnDestroyCoin(CoinPickup coin)
        {
            Destroy(coin.gameObject);
        }

        private void OnWaveCompleted(int wave)
        {
            foreach (CoinPickup coin in activeCoins)
            {
                coin.AttractTo(player);
            }
        }
    }
}
