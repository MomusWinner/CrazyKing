using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Controllers.Coins
{
    public class CoinsManager: IStartable
    {
        public int CurrentCoins { get; private set; }
        public Action<int, int> OnIncrease;
        public Action<int, int> OnDecrease;

        [Inject] private IObjectResolver _container;
        [Inject] private CoinsSO _coinsSO;
        private CoinPrice[] _coinPrices;
        
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
            _coinPrices = _coinsSO.CoinPrices;
            Array.Sort(_coinPrices, (a, b) =>
            {
                if (a.Price < b.Price) return 1;
                if (a.Price > b.Price) return -1;
                return 0;
            });
            Debug.Log("Initialize CoinsController");
        }

        public bool TryGetCoins(int coins)
        {
            if (CurrentCoins < coins) return false;

            CurrentCoins -= coins;
            SaveCoins();
            OnDecrease?.Invoke(CurrentCoins, coins);
            return true;
        }

        public void AddCoins(int coins)
        {
            CurrentCoins += coins;
            SaveCoins();
            OnIncrease?.Invoke(CurrentCoins, coins);
        }

        public List<Coin> GetCoins(int price)
        {
            List<Coin> coins = new List<Coin>();
            int remains = price;
            
            for (int i = 0; i < _coinPrices.Length; i++)
            {
                while (remains >= _coinPrices[i].Price)
                {
                    remains -= _coinPrices[i].Price;
                    coins.Add(CreateCoin(_coinPrices[i]));
                }
            }
            
            return coins;
        }

        private Coin CreateCoin(CoinPrice coinPrice)
        {
            var coinPref = Resources.Load<GameObject>(coinPrice.CoinPath);
            var coin = _container.Instantiate(coinPref).GetComponent<Coin>();
            coin.Price = coinPrice.Price;
            return coin;
        }
        
        private void SaveCoins()
        {
            PlayerPrefs.SetInt("Coins", CurrentCoins);
        }
    }
}