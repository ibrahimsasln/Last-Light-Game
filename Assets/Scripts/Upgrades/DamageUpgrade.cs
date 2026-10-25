using UnityEngine;
using Tilemania.Player;

namespace Tilemania.Upgrades
{
    [CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Tilemania/Upgrades/Damage")]
    public class DamageUpgrade : UpgradeDefinition
    {
        [SerializeField, Min(1)] int amount = 1;

        public override void Apply(PlayerUpgrades player)
        {
            player.Shooting.AddDamage(amount);
        }
    }
}
