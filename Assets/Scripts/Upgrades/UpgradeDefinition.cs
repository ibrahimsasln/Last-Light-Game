using UnityEngine;
using LastLight.Player;

namespace LastLight.Upgrades
{
    public abstract class UpgradeDefinition : ScriptableObject
    {
        [SerializeField] string displayName;
        [SerializeField, TextArea] string description;
        [SerializeField, Min(0)] int baseCost = 10;
        [Tooltip("Each level costs this many times the previous level.")]
        [SerializeField, Min(1f)] float costGrowth = 1.3f;
        [Tooltip("0 means the upgrade can be bought any number of times.")]
        [SerializeField, Min(0)] int maxLevel;

        public string DisplayName => displayName;
        public string Description => description;

        public int GetCost(int level)
        {
            return Mathf.RoundToInt(baseCost * Mathf.Pow(costGrowth, level));
        }

        public bool IsMaxed(int level, PlayerUpgrades player)
        {
            return (maxLevel > 0 && level >= maxLevel) || HasReachedLimit(player);
        }

        public string GetLevelText(int level)
        {
            if (maxLevel > 0) return $"{level}/{maxLevel}";
            return level > 0 ? $"Lv {level}" : string.Empty;
        }

        protected virtual bool HasReachedLimit(PlayerUpgrades player)
        {
            return false;
        }

        public virtual bool CanApply(PlayerUpgrades player)
        {
            return true;
        }

        public abstract void Apply(PlayerUpgrades player);
    }
}
