using TMPro;
using UnityEngine;
using Tilemania.Combat;
using Tilemania.Core;

namespace Tilemania.UI
{
    public class HealthDisplay : MonoBehaviour
    {
        TextMeshProUGUI healthText;
        Health playerHealth;

        private void Awake()
        {
            healthText = GetComponent<TextMeshProUGUI>();
            playerHealth = GameObject.FindWithTag(Tags.Player).GetComponent<Health>();
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
