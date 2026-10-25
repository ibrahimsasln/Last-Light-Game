using UnityEngine;
using Tilemania.Player;

namespace Tilemania.Upgrades
{
    [CreateAssetMenu(fileName = "MultiShotUpgrade", menuName = "Tilemania/Upgrades/Multi Shot")]
    public class MultiShotUpgrade : UpgradeDefinition
    {
        [SerializeField, Min(1)] int extraProjectiles = 1;

        public override void Apply(PlayerUpgrades player)
        {
            player.Shooting.AddProjectiles(extraProjectiles);
        }
    }
}
