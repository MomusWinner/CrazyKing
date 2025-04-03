using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Controllers.Coins
{
    public class CoinsManager: IStartable
    {
        private readonly int _startingCoins;
        public int CurrentCoins { get; private set; }
        public Action<int, int> OnIncrease;
        public Action<int, int> OnDecrease;

        [Inject] private IObjectResolver _container;
        [Inject] private CoinsSO _coinsSO;
        [Inject] private SaveManager _saveManager;
        private CoinPrice[] _coinPrices;
        
        public CoinsManager(int startingCoins)
        {
            _startingCoins = startingCoins;
        }
        
        public void Start()
        {
            if (_saveManager.GameData.IsFirstStarting)
                CurrentCoins = _startingCoins;
            
            _coinPrices = _coinsSO.CoinPrices;
            Array.Sort(_coinPrices, (a, b) =>
            {
                if (a.Price < b.Price) return 1;
                if (a.Price > b.Price) return -1;
                return 0;
            });
            Debug.Log("Initialize CoinsController");
        }

        public static string Short(long coin, int digits = 2)
        {
            switch (coin)
            {
                case >= 1000000:
                    return $"{Math.Round(coin / 1000000f, digits)}m";
                case >= 1000:
                    if (digits > 2)
                        return coin.ToString();
                    return $"{Math.Round(coin / 1000f, digits)}k";
                default:
                    return coin.ToString();
            }
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
        
        private void SaveCoins() {
            _saveManager.GameData.Coins = CurrentCoins;
        }
    }
}