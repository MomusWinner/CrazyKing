using King.Upgrades.Parameters;
using UI.Upgrade.KingParameters;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace UI.Upgrade.KingParametersTab
{
    public class KingParameterTab : MonoBehaviour
    {
        [SerializeField] private GameObject _kingParameterPref;
        [SerializeField] private GameObject _parametersParent;
        [Inject] private KingParametersSO _parameters;
        [Inject] private IObjectResolver _container;

        public void Start()
        {
            foreach (var (type, kingParameter) in _parameters.GetKingParameters())
            {
                var parameterUpController = _container.Instantiate(_kingParameterPref, _parametersParent.transform)
                    .GetComponent<KingParameterUI>();
                    parameterUpController.Setup(kingParameter);
            }
        }
    }
}