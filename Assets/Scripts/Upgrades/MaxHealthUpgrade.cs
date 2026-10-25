using UnityEngine;
using Tilemania.Player;

namespace Tilemania.Upgrades
{
    [CreateAssetMenu(fileName = "MaxHealthUpgrade", menuName = "Tilemania/Upgrades/Max Health")]
    public class MaxHealthUpgrade : UpgradeDefinition
    {
        [SerializeField, Min(1)] int amount = 1;

        public override void Apply(PlayerUpgrades player)
        {
            player.Health.IncreaseMaxHealth(amount);
        }
    }
}
