using UnityEngine;
using Tilemania.Player;

namespace Tilemania.Upgrades
{
    [CreateAssetMenu(fileName = "MoveSpeedUpgrade", menuName = "Tilemania/Upgrades/Move Speed")]
    public class MoveSpeedUpgrade : UpgradeDefinition
    {
        [SerializeField] float speedBonus = 0.1f;

        public override void Apply(PlayerUpgrades player)
        {
            player.Movement.AddSpeedBonus(speedBonus);
        }
    }
}
