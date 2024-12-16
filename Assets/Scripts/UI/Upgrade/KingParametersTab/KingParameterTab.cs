using King;
using UI.Upgrade.KingParameters;
using UnityEngine;
using VContainer;

namespace UI.Upgrade.KingParametersTab
{
    public class KingParameterTab : MonoBehaviour
    {
        [SerializeField] private GameObject _kingParameterPref;
        [SerializeField] private GameObject _parametersParent;
        [Inject] private KingController _king;

        public void Start()
        {
            foreach (var kingParameter in _king.KingParameterUpgradeControllers)
            {
                var parameterUpController = Instantiate(_kingParameterPref, _parametersParent.transform)
                    .GetComponent<KingParameter>();
                    parameterUpController.Setup(kingParameter);
            }
        }
    }
}