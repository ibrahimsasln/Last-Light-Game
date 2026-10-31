using UnityEngine;
using LastLight.Player;

namespace LastLight.Upgrades
{
    [CreateAssetMenu(fileName = "MaxHealthUpgrade", menuName = "Last Light/Upgrades/Max Health")]
    public class MaxHealthUpgrade : UpgradeDefinition
    {
        [SerializeField, Min(1)] int amount = 1;

        public override void Apply(PlayerUpgrades player)
        {
            player.Health.IncreaseMaxHealth(amount);
        }
    }
}
