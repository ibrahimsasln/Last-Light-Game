using TMPro;
using UnityEngine;
using Tilemania.Combat;

namespace Tilemania.UI
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] Health playerHealth;

        TextMeshProUGUI healthText;

        private void Awake()
        {
            healthText = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            playerHealth.HealthChanged += UpdateText;
        }

        private void OnDisable()
        {
            playerHealth.HealthChanged -= UpdateText;
        }

        private void Start()
        {
            UpdateText(playerHealth.CurrentHealth, playerHealth.MaxHealth);
        }

        private void UpdateText(int current, int max)
        {
            healthText.text = $"HP {current}/{max}";
        }
    }
}
