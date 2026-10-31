using UnityEngine;
using LastLight.Player;

namespace LastLight.Upgrades
{
    [CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Last Light/Upgrades/Damage")]
    public class DamageUpgrade : UpgradeDefinition
    {
        [SerializeField, Min(1)] int amount = 1;

        public override void Apply(PlayerUpgrades player)
        {
            player.Shooting.AddDamage(amount);
        }
    }
}
