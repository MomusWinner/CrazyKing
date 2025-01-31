using Controllers;
using Controllers.SoundManager;
using UnityEngine;
using VContainer;

namespace UI.Upgrade
{
    public class UpgradePanel : MonoBehaviour
    {
        [SerializeField] private GameObject positioningTab;
        [SerializeField] private GameObject upgradeTab;
        [SerializeField] private GameObject _settingPanel;
        [Inject] private LevelManager _levelManager;
        [Inject] private SoundManager _soundManager;
        
        private GameObject _currentTab;

        public void Start()
        {
            _soundManager.StartMusic("UpgradeMenuMusic", SoundChannel.Background);
            positioningTab.SetActive(false);
            upgradeTab.SetActive(false);
            OpenTab(positioningTab);
        }

        public void DeleteSave()
        {
            PlayerPrefs.DeleteAll();
        }

        public void StartGame()
        {
            _levelManager.LoadLevel();
        }

        public void OpenUpgradesTab() => OpenTab(upgradeTab);

        public void OpenPositioningTab() => OpenTab(positioningTab);
        
        private void OpenTab(GameObject tab)
        {
            _currentTab?.SetActive(false);
            _currentTab = tab;
            _currentTab.SetActive(true);
        }

        public void OpenSettings() => _settingPanel.SetActive(true);

        public void CloseSettings() => _settingPanel.SetActive(false);
    }
}