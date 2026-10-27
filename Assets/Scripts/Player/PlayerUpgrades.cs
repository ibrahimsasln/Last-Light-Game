using System;
using System.Collections.Generic;
using UnityEngine;
using Tilemania.Combat;
using Tilemania.Upgrades;

namespace Tilemania.Player
{
    public class PlayerUpgrades : MonoBehaviour
    {
        readonly Dictionary<UpgradeDefinition, int> levels = new Dictionary<UpgradeDefinition, int>();

        public Health Health { get; private set; }
        public PlayerWallet Wallet { get; private set; }
        public PlayerMovement Movement { get; private set; }
        public PlayerShooting Shooting { get; private set; }

        public event Action<UpgradeDefinition> Purchased;

        private void Awake()
        {
            Health = GetComponent<Health>();
            Wallet = GetComponent<PlayerWallet>();
            Movement = GetComponent<PlayerMovement>();
            Shooting = GetComponent<PlayerShooting>();
        }

        public int GetLevel(UpgradeDefinition upgrade)
        {
            return levels.TryGetValue(upgrade, out int level) ? level : 0;
        }

        public bool CanPurchase(UpgradeDefinition upgrade)
        {
            int level = GetLevel(upgrade);
            return !upgrade.IsMaxed(level, this) && upgrade.CanApply(this) && Wallet.Coins >= upgrade.GetCost(level);
        }

        public bool TryPurchase(UpgradeDefinition upgrade)
        {
            if (!CanPurchase(upgrade)) return false;

            int level = GetLevel(upgrade);
            Wallet.TrySpend(upgrade.GetCost(level));
            upgrade.Apply(this);
            levels[upgrade] = level + 1;

            Purchased?.Invoke(upgrade);
            return true;
        }
    }
}
