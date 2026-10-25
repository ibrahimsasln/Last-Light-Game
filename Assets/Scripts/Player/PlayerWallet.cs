using System;
using UnityEngine;

namespace Tilemania.Player
{
    public class PlayerWallet : MonoBehaviour
    {
        public int Coins { get; private set; }
        public event Action<int> CoinsChanged;

        public void Add(int amount)
        {
            Coins += amount;
            CoinsChanged?.Invoke(Coins);
        }

        public bool TrySpend(int amount)
        {
            if (amount > Coins) return false;

            Coins -= amount;
            CoinsChanged?.Invoke(Coins);
            return true;
        }
    }
}
