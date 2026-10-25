using System.Collections.Generic;
using UnityEngine;

namespace Tilemania.Upgrades
{
    [CreateAssetMenu(fileName = "ShopCatalog", menuName = "Tilemania/Shop Catalog")]
    public class ShopCatalog : ScriptableObject
    {
        [SerializeField] string shopName;
        [SerializeField] List<UpgradeDefinition> upgrades = new List<UpgradeDefinition>();

        public string ShopName => shopName;
        public IReadOnlyList<UpgradeDefinition> Upgrades => upgrades;
    }
}
