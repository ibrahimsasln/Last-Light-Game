using TMPro;
using UnityEngine;
using Tilemania.Player;

namespace Tilemania.UI
{
    public class CoinDisplay : MonoBehaviour
    {
        [SerializeField] PlayerWallet wallet;

        TextMeshProUGUI coinText;

        private void Awake()
        {
            coinText = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            wallet.CoinsChanged += UpdateText;
        }

        private void OnDisable()
        {
            wallet.CoinsChanged -= UpdateText;
        }

        private void Start()
        {
            UpdateText(wallet.Coins);
        }

        private void UpdateText(int coins)
        {
            coinText.text = coins.ToString();
        }
    }
}
