using UnityEngine;
using Tilemania.Player;

namespace Tilemania.Upgrades
{
    [CreateAssetMenu(fileName = "DodgeUpgrade", menuName = "Tilemania/Upgrades/Dodge")]
    public class DodgeUpgrade : UpgradeDefinition
    {
        public override void Apply(PlayerUpgrades player)
        {
            player.Movement.UnlockDodge();
        }
    }
}
