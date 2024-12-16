using King.Upgrades;
using TMPro;
using UnityEngine;

namespace UI.Upgrade.KingParameters
{
    public class KingParameter : MonoBehaviour
    {
        [SerializeField] private ProgressBarByCells _progressBar;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _price;
        
        private KingParameterUpgradeController _parameterController;
        
        public void Setup(KingParameterUpgradeController parameterController)
        {
            _parameterController = parameterController;
            _title.text = _parameterController.Title;
            _description.text = _parameterController.NextUpgradeDescription;
            _price.text = _parameterController.NextUpgradePrice.ToString();
            _progressBar.SetUp(_parameterController.Length);
            _progressBar.SetCurrentValue(1);
        }

        public void Upgrade()
        {
            _parameterController.Upgrade();
            if (_parameterController.NextUpgrade is null)
            {
                _progressBar.SetFullValue();
                _description.text = ". . .";
                _price.text = "-";
                return;
            }
            _description.text = _parameterController.NextUpgradeDescription;
            _price.text = _parameterController.NextUpgradePrice.ToString();
            _progressBar.SetCurrentValue(_parameterController.currentIndex + 2);
        }
    }
}