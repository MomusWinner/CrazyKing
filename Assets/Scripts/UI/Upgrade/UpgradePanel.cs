using Controllers;
using Controllers.Coins;
using Controllers.SoundManager;
using UnityEngine;
using VContainer;

namespace UI.Upgrade
{
    public class UpgradePanel : MonoBehaviour
    {
        [SerializeField] private UpgradeTab _positioningTab;
        [SerializeField] private UpgradeTab _upgradeTab;
        [SerializeField] private GameObject _settingPanel;
        [SerializeField] private GameObject[] _showInDebugMode;
        [Inject] private CoinsManager _coinsManager;
        [Inject] private LevelManager _levelManager;
        [Inject] private SoundManager _soundManager;
        
        private UpgradeTab _currentTab;

        public void Start()
        {
            _soundManager.StartMusic("UpgradeMenuMusic", SoundChannel.Background);
            _upgradeTab.gameObject.SetActive(true);
            _positioningTab.gameObject.SetActive(true);
            _positioningTab.HideNoAnim();
            OpenTab(_upgradeTab);

#if DEBUG
            foreach (var o in _showInDebugMode)
                o.SetActive(true);
#else
            foreach (var o in _showInDebugMode) 
                o.SetActive(false);
#endif
        }

        public void DeleteSave()
        {
            PlayerPrefs.DeleteAll();
        }

        public void AddCoins()
        {
            _coinsManager.AddCoins(1000);
        }

        public void StartGame()
        {
            _levelManager.LoadLevel();
        }

        public void OpenUpgradesTab() => OpenTab(_upgradeTab);

        public void OpenPositioningTab() => OpenTab(_positioningTab);
        
        private void OpenTab(UpgradeTab tab)
        {
            _currentTab?.Hide();
            _currentTab = tab;
            _currentTab.Show();
        }

        public void OpenSettings() => _settingPanel.SetActive(true);

        public void CloseSettings() => _settingPanel.SetActive(false);
    }
}