using UnityEngine;
using LastLight.Player;

namespace LastLight.Upgrades
{
    [CreateAssetMenu(fileName = "DodgeUpgrade", menuName = "Last Light/Upgrades/Dodge")]
    public class DodgeUpgrade : UpgradeDefinition
    {
        public override void Apply(PlayerUpgrades player)
        {
            player.Movement.UnlockDodge();
        }
    }
}
