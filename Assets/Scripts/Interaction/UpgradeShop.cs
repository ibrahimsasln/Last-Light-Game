using UnityEngine;
using LastLight.Managers;
using LastLight.Upgrades;

namespace LastLight.Interaction
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
