using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace UI
{
    public enum BuyButtonState
    {
        Enable,
        Disable,
        Super
    }
    
    public class BuyButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Sprite _disableSprite;
        [SerializeField] private Sprite _enableSprite;
        [SerializeField] private Sprite _superSprite;
        
        private TMP_Text _text;
        private Image _buttonImage;
        private Vector3 _startScale;
        private BuyButtonState _currentState;
        
        public void Awake()
        {
            _startScale = transform.localScale;
            _buttonImage = GetComponent<Image>();
            _text = GetComponentInChildren<TMP_Text>();
        }
        
        public void SetState(BuyButtonState state)
        {
            if (state == _currentState) return;
            _currentState = state;

            _buttonImage.sprite = state switch
            {
                BuyButtonState.Enable => _enableSprite,
                BuyButtonState.Disable => _disableSprite,
                BuyButtonState.Super => _superSprite,
                _ => _buttonImage.sprite
            };
        }

        public void SetText(string text)
        {
            _text.text = text;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (_currentState)
            {
                case BuyButtonState.Enable:
                    ScaleButton();
                    break;
                case BuyButtonState.Disable:
                    ShakeButton();
                    break;
                case BuyButtonState.Super:
                    ScaleButton();
                    break;
            }
        }
        
        public void ScaleButton()
        {
            DOTween.Kill(gameObject);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(_startScale * 1.1f, 0.3f).SetEase(Ease.OutCubic));
            sequence.Append(transform.DOScale(_startScale, 0.2f));
        }

        public void ShakeButton()
        {
            DOTween.Kill(gameObject);
            transform.DOShakePosition(0.4f, strength: 5f);
        }
    }
}