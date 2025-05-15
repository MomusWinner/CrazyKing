using Controllers.Coins;
using DG.Tweening;
using TMPro;
using UnityEngine;
using VContainer;

namespace UI
{
    public class CoinsUI : MonoBehaviour
    {
        [SerializeField] private float _animDuration = 0.5f;
        [SerializeField] private Color _decreaseColor = Color.red;
        [SerializeField] private Color _increaseColor = Color.green;
        [SerializeField] private TMP_Text coinText;
        [Inject] private CoinsManager _coinsManager;

        public void Start()
        {
            coinText.text = CoinsManager.Short(_coinsManager.CurrentCoins, 3);
            _coinsManager.OnIncrease += OnIncrease;
            _coinsManager.OnDecrease += OnDecrease;
        }

        private void OnIncrease(int coinsAmount, int increaseAmount)
        {
            DOTween.Kill(coinText.gameObject);
            coinText.text = CoinsManager.Short(coinsAmount, 3);
            coinText.transform
                .DOScale(Vector3.one * 1.2f, _animDuration)
                .SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    coinText.transform.DOScale(Vector3.one, _animDuration/2);
                });
            
            coinText.DOColor(_increaseColor, _animDuration)
                .SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    coinText.DOColor(Color.white, _animDuration/2);
                });
        }

        private void OnDecrease(int coinsAmount, int decreaseAmount)
        {
            DOTween.Kill(coinText.gameObject);
            coinText.text = CoinsManager.Short(coinsAmount, 3);
            coinText.transform
                .DOScale(Vector3.one * 1.2f, _animDuration)
                .SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    coinText.transform.DOScale(Vector3.one, _animDuration/2);
                });
            
            coinText.DOColor(_decreaseColor, _animDuration)
                .SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    coinText.DOColor(Color.white, _animDuration/2);
                });
        }

        public void OnDestroy()
        {
            _coinsManager.OnIncrease -= OnIncrease;
            _coinsManager.OnDecrease -= OnDecrease;
        }
    }
}