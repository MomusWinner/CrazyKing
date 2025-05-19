using DG.Tweening;
using UnityEngine;

namespace UI.Upgrade
{
    public class UpgradeTab : MonoBehaviour
    {
        public bool IsAnimating { get; private set; }
        [SerializeField] private RectTransform _leftPart;
        [SerializeField] private RectTransform _rightPart;
        [SerializeField] private RectTransform _closePointL;
        [SerializeField] private RectTransform _closePointR;
        private Vector3 _leftStartPosition;
        private Vector3 _rightStartPosition;
        private bool _isShow = true;

        public void Awake()
        {
            _leftStartPosition = _leftPart.position;
            _rightStartPosition = _rightPart.position;
        }

        public void Show()
        {
            if (_isShow) return;
            IsAnimating = true;
            _leftPart.DOMove(_leftStartPosition, 0.5f).SetEase(Ease.OutBack);
            _rightPart.DOMove(_rightStartPosition, 0.5f).SetEase(Ease.OutBack).onComplete = () => IsAnimating = false;
            _isShow = true;
        }

        public void Hide()
        {
            if (!_isShow) return;
            IsAnimating = true;
            _leftStartPosition = _leftPart.position;
            _rightStartPosition = _rightPart.position;
            _leftPart.DOMove(_closePointL.position, 0.5f);
            _rightPart.DOMove(_closePointR.position, 0.5f).onComplete = () => IsAnimating = false;
            _isShow = false;
        }
        
        public void HideNoAnim()
        {
            if (!_isShow) return;
            _leftPart.position = _closePointL.position;
            _rightPart.position = _closePointR.position;
            _isShow = false;
        }
    }
}