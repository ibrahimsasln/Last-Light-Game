using UnityEngine;
using LastLight.Player;

namespace LastLight.Upgrades
{
    [CreateAssetMenu(fileName = "MultiShotUpgrade", menuName = "Last Light/Upgrades/Multi Shot")]
    public class MultiShotUpgrade : UpgradeDefinition
    {
        [SerializeField, Min(1)] int extraProjectiles = 1;

        public override void Apply(PlayerUpgrades player)
        {
            player.Shooting.AddProjectiles(extraProjectiles);
        }
    }
}
