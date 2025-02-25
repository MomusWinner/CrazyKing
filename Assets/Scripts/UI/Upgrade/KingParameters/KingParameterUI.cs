using Controllers;
using Controllers.CoinsManager;
using Controllers.SoundManager;
using King;
using King.Upgrades.Parameters;
using TMPro;
using UnityEngine;
using VContainer;

namespace UI.Upgrade.KingParameters
{
    public class KingParameterUI : MonoBehaviour
    {
        [SerializeField] private ProgressBarByCells _progressBar;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _price;
        [Inject] private KingParameterManager _kingParameterManager;
        [Inject] private CoinsManager _coinsManager;
        [Inject] private SoundManager _soundManager;
        private KingParameter _kingParameter; 
        private int _currentLevel;
        private KingParameterUp _currentUpgradeData; 
        public void Setup(KingParameter kingParameter)
        {
            _currentLevel = _kingParameterManager.GetParameterLevel(kingParameter.type);
            _kingParameter = kingParameter; 
            _title.text = kingParameter.name;
            _progressBar.SetUp(kingParameter.Upgrades.Count);
            _progressBar.SetCurrentValue(1);
            ShowUpgradeData();
        }

        private void ShowUpgradeData()
        {
            if (IsMaxLevel())
            {
                _progressBar.SetFullValue();
                _description.text = ". . .";
                _price.text = "-";
                return;
            }
            _currentUpgradeData = _kingParameter.Upgrades[_currentLevel];
            _price.text = _currentUpgradeData.price.ToString();
            _description.text = _currentUpgradeData.description;
            _progressBar.SetCurrentValue(_currentLevel + 1);
        }
        
        public void UpgradeLevel()
        {
            if (IsMaxLevel()) return;
            UpgradeParamLevel(_kingParameter.type);
            ShowUpgradeData();
        }

        private bool IsMaxLevel()
        {
            return _kingParameter.Upgrades.Count <= _currentLevel;
        }
        
        private void UpgradeParamLevel(KingParameterType parameterType) 
        {
            if (!_coinsManager.TryGetCoins(_currentUpgradeData.price))
            {
                Debug.LogWarning("No coins available for upgrade");
                return;
            }
            _soundManager.StartMusic("Buy", SoundChannel.UI);
            _kingParameterManager.UpgradeParameter(_kingParameter.type);
            _currentLevel = _kingParameterManager.GetParameterLevel(parameterType);
        }
    }
}