using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tilemania.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public int Coins { get; private set; }
        public event Action<int> CoinsChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void AddCoins(int amount)
        {
            Coins += amount;
            CoinsChanged?.Invoke(Coins);
        }

        public void GameOver()
        {
            // TODO: show Game Over screen with restart / main menu buttons
            Coins = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
