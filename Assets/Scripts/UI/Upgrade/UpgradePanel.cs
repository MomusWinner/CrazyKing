using System;
using Controllers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace UI.Upgrade
{
    public class UpgradePanel : MonoBehaviour
    {
        [SerializeField] private GameObject positioningTab;
        [SerializeField] private GameObject upgradeTab;
        [Inject] private LevelManager _levelManager;
        
        private GameObject currentTab;

        public void Start()
        {
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
            currentTab?.SetActive(false);
            currentTab = tab;
            currentTab.SetActive(true);
        }
    }
}