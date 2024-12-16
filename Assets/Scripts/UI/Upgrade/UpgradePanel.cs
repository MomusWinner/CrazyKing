using DG.Tweening;
using UnityEngine;

namespace UI.Upgrade
{
    public class UpgradePanel : MonoBehaviour
    {
        [SerializeField] private float _panelWidth;
        private RectTransform _rectTransform;

        public void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Open()
        {
            _rectTransform.DOAnchorPosX(-1 * _panelWidth / 2, 1f).SetEase(Ease.OutCubic);
        }

        public void Close()
        {
            _rectTransform.DOAnchorPosX(_panelWidth / 2, 1f).SetEase(Ease.OutCubic);
        }
    }
}