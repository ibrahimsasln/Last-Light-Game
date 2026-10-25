using UnityEngine;
using Tilemania.Player;

namespace Tilemania.Upgrades
{
    public abstract class UpgradeDefinition : ScriptableObject
    {
        [SerializeField] string displayName;
        [SerializeField, TextArea] string description;
        [SerializeField, Min(0)] int baseCost = 10;
        [SerializeField, Min(0)] int costIncreasePerLevel = 5;
        [Tooltip("0 means the upgrade can be bought any number of times.")]
        [SerializeField, Min(0)] int maxLevel = 5;

        public string DisplayName => displayName;
        public string Description => description;

        public int GetCost(int level)
        {
            return baseCost + costIncreasePerLevel * level;
        }

        public bool IsMaxed(int level)
        {
            return maxLevel > 0 && level >= maxLevel;
        }

        public string GetLevelText(int level)
        {
            return maxLevel > 0 ? $"{level}/{maxLevel}" : string.Empty;
        }

        public virtual bool CanApply(PlayerUpgrades player)
        {
            return true;
        }

        public abstract void Apply(PlayerUpgrades player);
    }
}
