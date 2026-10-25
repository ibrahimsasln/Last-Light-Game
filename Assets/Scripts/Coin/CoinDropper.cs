using UnityEngine;

namespace Tilemania.Coin
{
    public class CoinDropper : MonoBehaviour
    {
        [SerializeField] GameObject coinPrefab;
        [SerializeField] float minDropForce = 3f;
        [SerializeField] float maxDropForce = 6f;

        public void DropCoins(Vector2 position, int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject coin = Instantiate(coinPrefab, position, Quaternion.identity);

                Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
                float force = Random.Range(minDropForce, maxDropForce);
                coin.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
            }
        }
    }
}
