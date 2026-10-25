using UnityEngine;
using Tilemania.Player;

namespace Tilemania.Upgrades
{
    [CreateAssetMenu(fileName = "HealUpgrade", menuName = "Tilemania/Upgrades/Heal")]
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
