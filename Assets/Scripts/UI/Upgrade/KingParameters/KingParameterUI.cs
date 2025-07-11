﻿using Controllers.Coins;
using Controllers.SoundManager;
using Entity.King;
using Entity.King.Upgrades.Parameters;
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
        [SerializeField] private BuyButton _buyButton;
        
        [Inject] private KingParameterManager _kingParameterManager;
        [Inject] private CoinsManager _coinsManager;
        [Inject] private SoundManager _soundManager;
        
        private KingParameter _kingParameter; 
        private int _currentLevel;
        private KingParameterUp _currentUpgradeData; 
        private bool _isDisable;
        
        public void Setup(KingParameter kingParameter)
        {
            _currentLevel = _kingParameterManager.GetParameterLevel(kingParameter.type);
            _kingParameter = kingParameter; 
            _title.text = kingParameter.name;
            _progressBar.SetUp(kingParameter.Upgrades.Count);
            _progressBar.SetCurrentValue(1);
            ShowUpgradeData();
        
            CheckBuyState(_coinsManager.CurrentCoins, 0);
            _coinsManager.OnIncrease += CheckBuyState;
            _coinsManager.OnDecrease += CheckBuyState;
        }

        public void OnDestroy()
        {
            _coinsManager.OnIncrease -= CheckBuyState;
            _coinsManager.OnDecrease -= CheckBuyState;
        }

        public void DisableBuyButton()
        {
            if (_isDisable) return;
            _isDisable = true;
            _buyButton.SetState(BuyButtonState.Disable);
        }

        public void EnableBuyButton()
        {
            if (!_isDisable) return;
            _isDisable = false;
            _buyButton.SetState(BuyButtonState.Enable);
        }
        
        public void UpgradeLevel()
        {
            if (IsMaxLevel()) return;
            UpgradeParamLevel(_kingParameter.type);
            ShowUpgradeData();
            CheckBuyState(_coinsManager.CurrentCoins, 0);
        }

        private void CheckBuyState(int coins, int _)
        {
            if (IsMaxLevel()) return;
            if (_currentUpgradeData.price > coins)
                DisableBuyButton();
            else
                EnableBuyButton();
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
        
        private void ShowUpgradeData()
        {
            if (IsMaxLevel())
            {
                _progressBar.SetFullValue();
                _description.text = "";
                _buyButton.SetText("MAX");
                DisableBuyButton();
                return;
            }
            
            _currentUpgradeData = _kingParameter.Upgrades[_currentLevel];
            _buyButton.SetText(CoinsManager.Short(_currentUpgradeData.price));
            _description.text = _currentUpgradeData.description;
            _progressBar.SetCurrentValue(_currentLevel + 1);
        }
        
    }
}