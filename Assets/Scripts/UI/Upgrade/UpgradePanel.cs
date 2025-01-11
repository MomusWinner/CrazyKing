using DG.Tweening;
using UnityEngine;

namespace UI.Upgrade
{
    public class UpgradePanel : MonoBehaviour
    {
        [SerializeField] private float _panelWidth;
        [SerializeField] private GameObject _kingParameterTab;
        [SerializeField] private GameObject _servantTab;
        
        private RectTransform _rectTransform;
        private GameObject _currentTab;

        public void DeleteSave()
        {
            PlayerPrefs.DeleteAll();
        }
        // public void Start()
        // {
        //     OpenKingParameterTab();
        //     _rectTransform = GetComponent<RectTransform>();
        // }
        //
        // public void Open()
        // {
        //     _rectTransform.DOAnchorPosX(-1 * _panelWidth / 2, 1f).SetEase(Ease.OutCubic);
        // }
        //
        // public void Close()
        // {
        //     _rectTransform.DOAnchorPosX(_panelWidth / 2, 1f).SetEase(Ease.OutCubic);
        // }
        //
        // public void OpenKingParameterTab()
        // {
        //     OpenTab(_kingParameterTab);
        // }
        //
        // public void OpenServantTab()
        // {
        //     OpenTab(_servantTab);
        // }
        //
        // public void OpenTab(GameObject tab)
        // {
        //     _currentTab?.SetActive(false);
        //     tab.SetActive(true);
        //     _currentTab = tab;
        // }
    }
}