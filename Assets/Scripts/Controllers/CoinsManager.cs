using System;
using UnityEngine;
using VContainer.Unity;

namespace Controllers
{
    public class CoinsManager: IStartable
    {
        public int CurrentCoins { get; private set; }
        public Action<int> OnIncrease;
        public Action<int> OnDecrease;
        
        public CoinsManager(int startingCoins)
        {
            if (!PlayerPrefs.HasKey("Coins"))
            {
                CurrentCoins = startingCoins;
                SaveCoins();
            }
            else
                CurrentCoins = PlayerPrefs.GetInt("Coins");
        }

        
        public void Start()
        {
            Debug.Log("Initialize CoinsController");
        }

        public bool TryGetCoins(int coins)
        {
            if (CurrentCoins < coins) return false;

            CurrentCoins -= coins;
            SaveCoins();
            OnDecrease?.Invoke(CurrentCoins);
            return true;
        }

        public void AddCoins(int coins)
        {
            CurrentCoins += coins;
            SaveCoins();
            OnIncrease?.Invoke(CurrentCoins);
        }
        
        private void SaveCoins()
        {
            PlayerPrefs.SetInt("Coins", CurrentCoins);
        }
    }
}