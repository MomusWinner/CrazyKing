using UnityEngine;
using VContainer.Unity;

namespace Controllers
{
    public class CoinsController: IStartable
    {
        public int CurrentCoins { get; private set; }

        public void Start()
        {
            CurrentCoins = 100;
            Debug.Log("Initialize CoinsController");
        }

        public bool TryGetCoins(int coins)
        {
            Debug.Log(CurrentCoins);
            if (CurrentCoins < coins) return false;

            CurrentCoins -= coins;
            return true;
        }

        public void AddCoins(int coins)
        {
            CurrentCoins += coins;
        }
    }
}