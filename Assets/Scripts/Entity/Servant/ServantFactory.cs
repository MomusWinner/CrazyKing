using Entity.Servant.Archer;
using Entity.Servant.Archer.Upgrades;
using Entity.Servant.Knight;
using Entity.Servant.Knight.Upgrades;
using Servant;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Entity.Servant
{
    public class ServantFactory
    {
        private readonly IObjectResolver _container;
        private readonly ServantsSO _servantsSO;

        [Inject]
        private ServantFactory(IObjectResolver container, ServantsSO servantsSo)
        {
            _container = container;
            _servantsSO = servantsSo;
        }
        
        public ServantController CreateServant(ServantData servantData, Vector2 position)
        {
            ServantSO servantSO = _servantsSO.GetServantByType(servantData.Type);
            if (servantSO == null)
            {
                Debug.LogError($"Servant with type {servantData.Type} isn't registered in the servantsSO");
                return null;
            }
            
            var servant = Resources.Load<GameObject>(servantSO.prefabPath);
            ServantController servantController =  _container.Instantiate(
                servant,
                position,
                Quaternion.identity
            ).GetComponent<ServantController>();
            servantController.ServantData = servantData;
            servantController.ServantSO = servantSO;
            servantController.Initialize();
            IServantParameterSetter upgrader = GetParamUpgrader(servantController, servantSO);
            if (upgrader is null)
            {
                Debug.LogError($"Register your servant{servantSO.type} in GetParamUpgrader method");
            }
            upgrader.UpgradeParameters(servantData.Lv);
            servantController.StartFirstState();
            return servantController;
        }

        public IServantParameterSetter GetParamUpgrader(ServantController controller, ServantSO servantSo)
        {
            switch (servantSo.type)
            {
                case ServantType.Knight:
                    return new KnightParametersUpgrader((KnightServantSO)servantSo, (KnightController)controller);
                case ServantType.Archer:
                    return new ArcherParametersUpgrader((ArcherServantSO) servantSo, (ArcherController)controller);
            }
            return null;
        }
    }
}