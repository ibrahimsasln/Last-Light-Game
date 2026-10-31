using UnityEngine;
using LastLight.Player;

namespace LastLight.Upgrades
{
    [CreateAssetMenu(fileName = "FireRateUpgrade", menuName = "Last Light/Upgrades/Fire Rate")]
    public class FireRateUpgrade : UpgradeDefinition
    {
        [SerializeField, Range(0.1f, 1f)] float cooldownMultiplier = 0.85f;

        protected override bool HasReachedLimit(PlayerUpgrades player)
        {
            return player.Shooting.IsAtMinCooldown;
        }

        public override void Apply(PlayerUpgrades player)
        {
            player.Shooting.MultiplyCooldown(cooldownMultiplier);
        }
    }
}
