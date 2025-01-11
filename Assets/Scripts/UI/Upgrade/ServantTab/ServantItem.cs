using Servant;
using Servant.Upgrade;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Upgrade.ServantTab
{
    public class ServantItem: MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private Image _actionButtonImage;
        [SerializeField] private Image _icon;
        [SerializeField] private ProgressBarByCells _progressBar;
        [SerializeField] private TMP_Text _buttonText;
        private ServantController _servantController;
        
        public void Setup(ServantController servantController)
        {
            _servantController = servantController;
            _name.text = servantController.ServantData.servantName;
            _icon.sprite = servantController.ServantData.avatar;
            _progressBar.SetUp(servantController.UpgradeController.MaxLevel);
            _progressBar.SetCurrentValue(servantController.UpgradeController.Level);
            ServantUpgradeData nextUpgradeData = servantController.UpgradeController.GetNextUpgradeData();
            _buttonText.text = nextUpgradeData.Price.ToString();
            _description.text = nextUpgradeData.Description;
        }

        public void Upgrade()
        {
            _servantController.UpgradeController.Upgrade();
            if(_servantController.UpgradeController.Level == _servantController.UpgradeController.MaxLevel)
                _progressBar.SetFullValue();
            else
                _progressBar.SetCurrentValue(_servantController.UpgradeController.Level + 1);
        }
    }
}