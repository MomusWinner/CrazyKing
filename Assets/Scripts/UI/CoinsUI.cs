using Controllers;
using TMPro;
using UnityEngine;
using VContainer;

namespace UI
{
    public class CoinsUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text coinText;
        [Inject] private CoinsManager _coinsManager;

        public void Start()
        {
            coinText.text = _coinsManager.CurrentCoins.ToString();
            _coinsManager.OnIncrease += OnIncrease;
            _coinsManager.OnDecrease += OnDecrease;
        }

        private void OnIncrease(int coinsAmount)
        {
            coinText.text = coinsAmount.ToString();
        }

        private void OnDecrease(int coinsAmount)
        {
            coinText.text = coinsAmount.ToString();
        }
    }
}