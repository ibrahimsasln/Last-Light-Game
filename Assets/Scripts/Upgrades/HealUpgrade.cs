using UnityEngine;
using LastLight.Player;

namespace LastLight.Upgrades
{
    [CreateAssetMenu(fileName = "HealUpgrade", menuName = "Last Light/Upgrades/Heal")]
    public class HealUpgrade : UpgradeDefinition
    {
        public override bool CanApply(PlayerUpgrades player)
        {
            return !player.Health.IsFullHealth;
        }

        public override void Apply(PlayerUpgrades player)
        {
            player.Health.HealToFull();
        }
    }
}
