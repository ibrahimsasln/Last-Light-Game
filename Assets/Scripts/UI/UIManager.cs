using System;
using UnityEngine;
using Tilemania.Player;
using Tilemania.Upgrades;

namespace Tilemania.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] UITheme theme;

        MenuWindow window;

        public event Action ButtonClicked;

        private void Awake()
        {
            Canvas canvas = UIFactory.CreateCanvas("Menu Canvas", 100);
            canvas.transform.SetParent(transform, false);

            window = new MenuWindow(canvas.transform, theme);
            window.ButtonClicked += OnWindowButtonClicked;
        }

        private void OnDestroy()
        {
            window.ButtonClicked -= OnWindowButtonClicked;
        }

        public void ShowMainMenu(int bestWave, Action onPlay, Action onSettings, Action onQuit)
        {
            window.Open("TILEMANIA");
            window.AddText(bestWave > 0 ? $"Best wave: {bestWave}" : "Survive as many waves as you can!");
            window.AddText("A / D: move   Space: jump   Mouse: aim and shoot   E: interact   Esc / P: pause", theme.TextSize * 0.6f, theme.TextColor);
            window.AddButton("Play", onPlay);
            window.AddButton("Settings", onSettings);

            if (onQuit != null)
            {
                window.AddButton("Quit", onQuit);
            }
        }

        public void ShowPauseMenu(Action onResume, Action onSettings, Action onMainMenu)
        {
            window.Open("Paused");
            window.AddButton("Resume", onResume);
            window.AddButton("Settings", onSettings);
            window.AddButton("Main Menu", onMainMenu);
        }

        public void ShowGameOver(int wave, int bestWave, bool isNewBest, Action onRestart, Action onMainMenu)
        {
            window.Open("Game Over");
            window.AddText($"You reached wave {wave}");
            window.AddText(isNewBest ? "New best!" : $"Best wave: {bestWave}", theme.TextSize, theme.AccentColor);
            window.AddButton("Play Again", onRestart);
            window.AddButton("Main Menu", onMainMenu);
        }

        public void ShowSettings(float volume, Action<float> onVolumeChanged, Action onBack)
        {
            window.Open("Settings");
            window.AddText("Volume");
            window.AddSlider(volume, onVolumeChanged);
            window.AddButton("Back", onBack);
        }

        public void ShowShop(ShopCatalog catalog, PlayerUpgrades player, Action<UpgradeDefinition> onBuy, Action onClose)
        {
            window.Open(catalog.ShopName);
            window.AddText($"Coins: {player.Wallet.Coins}", theme.TextSize, theme.AccentColor);

            foreach (UpgradeDefinition upgrade in catalog.Upgrades)
            {
                int level = player.GetLevel(upgrade);
                string price = upgrade.IsMaxed(level, player) ? "MAX" : $"{upgrade.GetCost(level)} coins";
                string label = $"{upgrade.DisplayName} {upgrade.GetLevelText(level)}  -  {price}\n<size=65%>{upgrade.Description}</size>";

                window.AddButton(label, () => onBuy(upgrade), player.CanPurchase(upgrade), 1.5f);
            }

            window.AddButton("Close", onClose);
        }

        public void HideMenu()
        {
            window.Close();
        }

        private void OnWindowButtonClicked()
        {
            ButtonClicked?.Invoke();
        }
    }
}
