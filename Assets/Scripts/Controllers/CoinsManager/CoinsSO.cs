using System;
using UnityEngine;

namespace Controllers.CoinsManager
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