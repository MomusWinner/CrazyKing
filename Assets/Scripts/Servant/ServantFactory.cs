using System.Linq;
using King;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Servant
{
    public class ServantFactory
    {
        private readonly IObjectResolver _container;
        private readonly KingController _king;

        private ServantSO _knight;
        
        [Inject]
        private ServantFactory(IObjectResolver container, KingController king, ServantsSO servantsSo)
        {
            _container = container;
            _king = king;
            _knight = servantsSo.availableServants.First();
        }
        
        public void CreateKnight(Vector2 position)
        {
            CreateServant(_knight);
        }

        public ServantController CreateServant(ServantSO servantSO)
        {
            if (!_king.TryGetFreePoint(out IPoint point))
            {
                Debug.LogWarning("Couldn't get free point from the king");
                return null;
            }
            var servant = Resources.Load<GameObject>(servantSO.prefabPath);
            ServantController servantController =  _container.Instantiate(
                servant,
                point.GetPosition(),
                Quaternion.identity
            ).GetComponent<ServantController>();
            servantController.Point = point;
            servantController.ServantData = servantSO;
            return servantController;
        }
    }
}