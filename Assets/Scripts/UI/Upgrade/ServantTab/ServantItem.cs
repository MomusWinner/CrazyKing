using System;
using System.Collections.Generic;
using System.Linq;
using Controllers.Coins;
using Controllers.SoundManager;
using DG.Tweening;
using Servant;
using Servant.Upgrade;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace UI.Upgrade.ServantTab
{
    public class ServantItem: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public bool ParameterIsOpen { get; set; } = false;
        public Action<ServantItem> OnPointerEnterEvent { get; set; }
        public Action<ServantItem> OnPointerExitEvent { get; set; }
        public Action<ServantItem> OnPointerClickEvent  { get; set; }

        public ServantData ServantData => _servantData;
        
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _lvText;
        [SerializeField] private Image _actionButtonImage;
        [SerializeField] private Button _actionButton;
        [SerializeField] private Image _icon;
        [SerializeField] private ProgressBarByCells _progressBar;
        [SerializeField] private TMP_Text _buttonText;
        [SerializeField] private RectTransform _paramParent;
        
        [Inject] private ServantsSO _servantsSO;
        [Inject] private ServantStorage _servantStorage;
        [Inject] private CoinsManager _coinsManager;
        [Inject] private SoundManager _soundManager;
        
        private ServantData _servantData;
        private ServantSO _servantSo;
        private IServantParameterContainer _parameterContainer;
        private ServantUpgradeData NextUpgrade => ServantData.Lv >= _servantSo.Upgrades.Count ? null : _servantSo.Upgrades[ServantData.Lv];
        private bool _canByMerged;
        private List<ServantData> _sameServants;
        private readonly List<ServantParameterUI> _params = new();
        private RectTransform _rectTransform;
        
        public void Setup(ServantData servantData)
        {
            _rectTransform = GetComponent<RectTransform>();
            
            _servantData = servantData;
            _servantSo = _servantsSO.GetServantByType(servantData.Type);
            
            // Setup parameters
            _parameterContainer = _servantSo as IServantParameterContainer;
            if (_parameterContainer != null)
            {
                foreach (var paramName in _parameterContainer.GetAvailableParameters())
                {
                    GameObject paramObj = Resources.Load<GameObject>("UI/UpgradePanel/ServantParameter");
                    ServantParameterUI param = Instantiate(paramObj, _paramParent.transform).GetComponent<ServantParameterUI>();
                    ServantParameterSO parameterSO = _servantsSO.Parameters.Find(p => p.FieldName == paramName);
                    if (parameterSO == null)
                    {
                        Debug.LogError($"Servant parameter {paramName} is not defined in ServantsSO");
                        continue;
                    }
                    param.Setup(paramName,
                        parameterSO.FriendlyName,
                        "",
                        parameterSO.Icon);
                    _params.Add(param);
                }
                UpdateServantParams();
            }
            _name.text = _servantSo.servantName;
            _progressBar.SetUp(_servantsSO.IntervalOfEvolutionLevels);
            _progressBar.SetFullValue();
            if (!IsMaxLv())
            {
                _servantStorage.OnUpgradeServant += OnUpgradeServant;
                CheckMergeAvailable();
            }
            ShowServantUpgradeData();
        }

        private void UpdateServantParams()
        {
            if(_parameterContainer == null || _params is null) return;
            foreach (var param in _params)
            {
                string value = _parameterContainer.GetParameterValue(param.FieldName, _servantData.Lv).ToString();

                if (_servantData.Lv < _servantSo.Upgrades.Count)
                {
                    string nextValue = _parameterContainer.GetParameterValue(param.FieldName, _servantData.Lv + 1).ToString();
                    if (nextValue != value)
                        value += $" -> <color=green><b>{nextValue}</b></color>";
                }
                
                param.UpdateText(value);
            }
        }

        public void OpenParameters()
        {
            if (ParameterIsOpen) return;
            
            ParameterIsOpen = true;
            float width = _paramParent.rect.width;
            _paramParent.transform.DOLocalMoveX(- width - _rectTransform.rect.width / 2, 0.4f).SetEase(Ease.OutQuart);
        }

        public void CloseParameters()
        {
            if (!ParameterIsOpen) return;
            ParameterIsOpen = false;
            _paramParent.transform.DOLocalMoveX(- _rectTransform.rect.width / 2, 0.4f);
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
                    UpdateServantParams();
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
                UpdateServantParams();
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
            _lvText.text = _servantData.Lv.ToString();
            SetProgressBarValue(_servantData.Lv);
            if (IsMaxLv())
            {
                _buttonText.text = "MAX";
                _description.text = "max lv";
                _actionButton.interactable = false;
                if (_servantData.Lv % _servantsSO.IntervalOfEvolutionLevels == 0)
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
            if (lv % _servantsSO.IntervalOfEvolutionLevels == 0)
                _progressBar.SetCurrentValue(0);
            else
                _progressBar.SetCurrentValue(lv % _servantsSO.IntervalOfEvolutionLevels + 1);
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

        public void OnPointerEnter(PointerEventData eventData) => OnPointerEnterEvent?.Invoke(this);

        public void OnPointerExit(PointerEventData eventData) => OnPointerExitEvent?.Invoke(this);

        public void OnPointerClick(PointerEventData eventData) => OnPointerClickEvent?.Invoke(this);
    }
}