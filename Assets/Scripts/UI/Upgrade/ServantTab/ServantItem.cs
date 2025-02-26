using System.Collections.Generic;
using System.Linq;
using Controllers;
using Controllers.Coins;
using Controllers.SoundManager;
using Servant;
using Servant.Upgrade;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Upgrade.ServantTab
{
    public class ServantItem: MonoBehaviour
    {
        public ServantData ServantData => _servantData;
        
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _lvText;
        [SerializeField] private Image _actionButtonImage;
        [SerializeField] private Button _actionButton;
        [SerializeField] private Image _icon;
        [SerializeField] private ProgressBarByCells _progressBar;
        [SerializeField] private TMP_Text _buttonText;
        [Inject] private ServantsSO _servantsSO;
        [Inject] private ServantStorage _servantStorage;
        [Inject] private CoinsManager _coinsManager;
        [Inject] private SoundManager _soundManager;
        private ServantData _servantData;
        private ServantSO _servantSo;
        private ServantUpgradeData NextUpgrade => ServantData.Lv >= _servantSo.Upgrades.Count ? null : _servantSo.Upgrades[ServantData.Lv];
        private bool _canByMerged;
        private List<ServantData> _sameServants;
        
        public void Setup(ServantData servantData)
        {
            _servantData = servantData;
            _servantSo = _servantsSO.GetServantByType(servantData.Type);
            _name.text = _servantSo.servantName;
            _progressBar.SetUp(_servantsSO.intervalOfEvolutionLevels);
            _progressBar.SetFullValue();
            if (!IsMaxLv())
            {
                _servantStorage.OnUpgradeServant += OnUpgradeServant;
                CheckMergeAvailable();
            }
            ShowServantUpgradeData();
        }

        public void Upgrade()
        {
            if (NextUpgrade.isMergeUpgrade)
            {
                if (!_canByMerged)
                {
                    _soundManager.StartMusic("Block", SoundChannel.UI);
                    Debug.LogWarning("Can't merge servants.");
                    return;
                }

                if (_servantStorage.TryMergeServants(_sameServants
                        .Select(s => s.ID)
                        .ToList()
                        .GetRange(0, NextUpgrade.mergeAmount)))
                {
                    _soundManager.StartMusic("Buy", SoundChannel.UI);
                }
            }
            else
            {
                if (!_coinsManager.TryGetCoins(NextUpgrade.price))
                {
                    _soundManager.StartMusic("Block", SoundChannel.UI);
                    Debug.LogWarning("Not enough coins.");
                    return;
                } 
                _soundManager.StartMusic("Buy", SoundChannel.UI);
                _servantStorage.UpgradeServant(_servantData.ID);
            }

            if (IsMaxLv())
            {
                _servantStorage.OnUpgradeServant -= OnUpgradeServant;
            }
            ShowServantUpgradeData();
        }
        
        private void ShowServantUpgradeData()
        {
            if(_servantData is null) return;
            _icon.sprite = _servantSo.GetAvatarByLevel(_servantData.Lv);
            _lvText.text = $"lv {_servantData.Lv}";
            SetProgressBarValue(_servantData.Lv);
            if (IsMaxLv())
            {
                _buttonText.text = "MAX";
                _description.text = "max lv";
                _actionButton.interactable = false;
                if (_servantData.Lv % _servantsSO.intervalOfEvolutionLevels == 0)
                {
                    _progressBar.SetFullValue();
                }
                return;
            }
            _description.text = NextUpgrade.description;
            if (NextUpgrade.isMergeUpgrade)
                _buttonText.text = "MERGE " + NextUpgrade.mergeAmount;
            else
                _buttonText.text = NextUpgrade.price.ToString();
        }

        private void OnUpgradeServant(ServantData data)
        {
            if (data.ID == ServantData.ID)
            { 
                ShowServantUpgradeData();                   
            }
            CheckMergeAvailable();
        }

        private void SetProgressBarValue(int lv)
        {
            if (lv % _servantsSO.intervalOfEvolutionLevels == 0)
                _progressBar.SetCurrentValue(0);
            else
                _progressBar.SetCurrentValue(lv % _servantsSO.intervalOfEvolutionLevels + 1);
        }

        private bool IsMaxLv()
        {
            return _servantSo.Upgrades.Count <= _servantData.Lv;
        }

        private void CheckMergeAvailable()
        {
            if(NextUpgrade is null) return;
            if (!NextUpgrade.isMergeUpgrade)
            {
                _sameServants?.Clear();
                _canByMerged = false;
                return;
            }
            _sameServants = GetServantsBySameTypeAndLv();
            if (_sameServants.Count < NextUpgrade.mergeAmount)
            {
                _sameServants.Clear();
                _canByMerged = false;
                return;
            }
            _canByMerged = true;
        }

        private List<ServantData> GetServantsBySameTypeAndLv()
        {
            List<ServantData> servants = new List<ServantData>();
            foreach (var servantData in _servantStorage.Servants)
            {
                if (servantData.Lv == ServantData.Lv && servantData.Type == ServantData.Type)
                    servants.Add(servantData);
            }
            
            return servants;
        }
    }
}