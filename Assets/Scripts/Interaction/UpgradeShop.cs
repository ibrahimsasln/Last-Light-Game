using UnityEngine;
using Tilemania.Managers;
using Tilemania.Upgrades;

namespace Tilemania.Interaction
{
    public class UpgradeShop : BetweenWavesInteractable
    {
        [SerializeField] ShopCatalog catalog;
        [SerializeField] GameManager gameManager;

        protected override void OnInteract()
        {
            gameManager.OpenShop(catalog);
        }
    }
}
