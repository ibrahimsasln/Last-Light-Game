using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Tilemania.Audio;
using Tilemania.Combat;
using Tilemania.Player;
using Tilemania.UI;
using Tilemania.Upgrades;

namespace Tilemania.Managers
{
    public class GameManager : MonoBehaviour
    {
        const string GameplayActionMap = "Player";
        const string MenuActionMap = "UI";

        static bool skipMainMenuOnLoad;

        [Header("References")]
        [SerializeField] Health playerHealth;
        [SerializeField] PlayerInput playerInput;
        [SerializeField] PlayerUpgrades playerUpgrades;
        [SerializeField] WaveManager waveManager;

        [Header("Timing")]
        [SerializeField] float gameOverDelay = 2f;

        UIManager ui;
        AudioManager audioManager;
        HighScoreTable highScores;
        ShopCatalog openShop;

        public GameState State { get; private set; }
        public event Action<GameState> StateChanged;

        static bool CanQuit => Application.platform != RuntimePlatform.WebGLPlayer;

        private void Awake()
        {
            ui = GetComponent<UIManager>();
            audioManager = GetComponent<AudioManager>();
            highScores = new HighScoreTable();
        }

        private void OnEnable()
        {
            playerHealth.Died += OnPlayerDied;
        }

        private void OnDisable()
        {
            playerHealth.Died -= OnPlayerDied;
        }

        private void Start()
        {
            if (skipMainMenuOnLoad)
            {
                skipMainMenuOnLoad = false;
                StartPlaying();
            }
            else
            {
                ShowMainMenu();
            }
        }

        private void Update()
        {
            if (!WasPausePressed()) return;

            switch (State)
            {
                case GameState.Playing:
                    Pause();
                    break;
                case GameState.Paused:
                case GameState.Shopping:
                    Resume();
                    break;
            }
        }

        public void OpenShop(ShopCatalog catalog)
        {
            if (State != GameState.Playing) return;

            openShop = catalog;
            SetState(GameState.Shopping);
            ShowShop();
        }

        private void ShowMainMenu()
        {
            SetState(GameState.MainMenu);
            ShowMainMenuScreen();
        }

        private void ShowMainMenuScreen()
        {
            ui.ShowMainMenu(highScores.BestWave, StartPlaying, () => ShowSettings(ShowMainMenuScreen),
                () => ShowHighScores(-1, ShowMainMenuScreen), CanQuit ? Quit : null);
        }

        private void StartPlaying()
        {
            ui.HideMenu();
            SetState(GameState.Playing);
        }

        private void Pause()
        {
            SetState(GameState.Paused);
            ShowPauseScreen();
        }

        private void ShowPauseScreen()
        {
            ui.ShowPauseMenu(Resume, () => ShowSettings(ShowPauseScreen), ReturnToMainMenu);
        }

        private void Resume()
        {
            openShop = null;
            ui.HideMenu();
            SetState(GameState.Playing);
        }

        private void ShowShop()
        {
            ui.ShowShop(openShop, playerUpgrades, Buy, Resume);
        }

        private void Buy(UpgradeDefinition upgrade)
        {
            if (playerUpgrades.TryPurchase(upgrade))
            {
                ShowShop();
            }
        }

        private void ShowSettings(Action onBack)
        {
            ui.ShowSettings(audioManager.Volume, volume => audioManager.Volume = volume, () =>
            {
                audioManager.SaveSettings();
                onBack();
            });
        }

        private void OnPlayerDied()
        {
            StartCoroutine(GameOverAfterDelay());
        }

        private IEnumerator GameOverAfterDelay()
        {
            yield return new WaitForSeconds(gameOverDelay);

            int wave = waveManager.CurrentWave;
            int rank = highScores.Add(wave);

            SetState(GameState.GameOver);
            audioManager.PlayGameOver();
            ShowGameOverScreen(wave, rank);
        }

        private void ShowGameOverScreen(int wave, int rank)
        {
            ui.ShowGameOver(wave, rank, highScores.BestWave, Restart,
                () => ShowHighScores(rank, () => ShowGameOverScreen(wave, rank)), ReturnToMainMenu);
        }

        private void ShowHighScores(int highlightedRank, Action onBack)
        {
            ui.ShowHighScores(highScores.Entries, highlightedRank, onBack);
        }

        private void Restart()
        {
            skipMainMenuOnLoad = true;
            ReloadScene();
        }

        private void ReturnToMainMenu()
        {
            skipMainMenuOnLoad = false;
            ReloadScene();
        }

        private void Quit()
        {
            Application.Quit();
        }

        private void ReloadScene()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void SetState(GameState newState)
        {
            State = newState;

            bool isPlaying = newState == GameState.Playing;
            Time.timeScale = isPlaying ? 1f : 0f;
            playerInput.SwitchCurrentActionMap(isPlaying ? GameplayActionMap : MenuActionMap);

            StateChanged?.Invoke(newState);
        }

        private static bool WasPausePressed()
        {
            bool keyboardPressed = Keyboard.current != null
                && (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame);
            bool gamepadPressed = Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame;
            return keyboardPressed || gamepadPressed;
        }
    }
}
