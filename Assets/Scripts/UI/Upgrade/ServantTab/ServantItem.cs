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
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _lvText;
        [SerializeField] private Image _actionButtonImage;
        [SerializeField] private Image _icon;
        [SerializeField] private ProgressBarByCells _progressBar;
        [SerializeField] private TMP_Text _buttonText;
        [Inject] private ServantsSO _servantsSO;
        [Inject] private ServantsStorage _servantsStorage;
        private ServantData _servantData;
        private ServantSO _servantSo;
        
        public void Setup(ServantData servantData)
        {
            _servantData = servantData;
            _servantSo = _servantsSO.GetServantByType(servantData.Type);
            _name.text = _servantSo.servantName;
            _icon.sprite = _servantSo.avatar;
            _progressBar.SetUp(_servantsSO.intervalOfEvolutionLevels);
            ShowServantUpgradeData();
        }

        public void Upgrade()
        {
            _servantsStorage.UpgradeServant(_servantData.ID);
            ShowServantUpgradeData();
        }
        
        private void ShowServantUpgradeData()
        {
            _lvText.text = $"lv {_servantData.Lv}";
            if (IsMaxLv())
            {
                _buttonText.text = "MAX";
                _description.text = "max lv";
                if (_servantData.Lv % _servantsSO.intervalOfEvolutionLevels == 0)
                {
                    _progressBar.SetFullValue();
                }
                return;
            }
            ServantUpgradeSO upgradeSo = _servantSo.upgrades[_servantData.Lv];
            SetProgressBarValue(_servantData.Lv);
            _buttonText.text = upgradeSo.price.ToString();
            _description.text = upgradeSo.description;
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
            return _servantSo.upgrades.Count <= _servantData.Lv;
        }
    }
}