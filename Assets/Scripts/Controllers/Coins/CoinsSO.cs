using System;
using UnityEngine;

namespace Controllers.Coins
{
    [Serializable]
    public class CoinPrice
    {
        public int Price;
        public string CoinPath;
    }
    
    [CreateAssetMenu(menuName = "Game/" + nameof(CoinsSO))]
    public class CoinsSO : ScriptableObject
    {
        public CoinPrice[] CoinPrices;
    }
}