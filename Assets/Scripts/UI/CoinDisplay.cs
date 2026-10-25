using TMPro;
using UnityEngine;
using Tilemania.Managers;

namespace Tilemania.UI
{
    public class CoinDisplay : MonoBehaviour
    {
        TextMeshProUGUI coinText;

        private void Awake()
        {
            coinText = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            GameManager.Instance.CoinsChanged += UpdateText;
            UpdateText(GameManager.Instance.Coins);
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CoinsChanged -= UpdateText;
            }
        }

        private void UpdateText(int coins)
        {
            coinText.text = coins.ToString();
        }
    }
}
