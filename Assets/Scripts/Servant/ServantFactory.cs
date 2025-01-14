using King;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Servant
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
            return servantController;
        }
    }
}