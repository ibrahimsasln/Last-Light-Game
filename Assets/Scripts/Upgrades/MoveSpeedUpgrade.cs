using UnityEngine;
using LastLight.Player;

namespace LastLight.Upgrades
{
    [CreateAssetMenu(fileName = "MoveSpeedUpgrade", menuName = "Last Light/Upgrades/Move Speed")]
    public class MoveSpeedUpgrade : UpgradeDefinition
    {
        [SerializeField] float speedBonus = 0.1f;

        public override void Apply(PlayerUpgrades player)
        {
            player.Movement.AddSpeedBonus(speedBonus);
        }
    }
}
