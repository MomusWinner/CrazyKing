using System.Collections.Generic;
using Entity.King;
using Servant;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Entity.Servant
{
    public class ServantManager : IStartable
    {
        public bool _isInitialized = false;
        public IReadOnlyCollection<ServantController> Servants => _servants.AsReadOnly(); 
        [Inject] private ServantFactory _servantFactory;
        [Inject] private ServantStorage _servantStorage;
        [Inject] private KingController _king;
        
        private readonly List<ServantController> _servants = new ();
        private readonly List<ServantData> _captiveServants = new();
        
        public void Start()
        {
            LoadKingServants();
            _isInitialized = true;
        }

        public ServantController CreateCaptiveServant(ServantType type, Vector3 position)
        {
            var servantData = _servantStorage.CreateServantData(type);
            ServantController servant = _servantFactory.CreateServant(servantData, position);
            _captiveServants.Add(servantData);
            return servant;
        }

        public void AddCaptiveServants()
        {
            foreach (var servantData in _captiveServants)
                _servantStorage.AddServant(servantData);
        }

        public void ClearCaptiveServants()
        {
            _captiveServants.Clear();
        }
        
        private void LoadKingServants()
        {
            foreach (var servantData in _servantStorage.Servants)
            {
                if (!servantData.IsUsed) continue;

                if (!_king.TryGetPositionById(servantData.PointId, out var position))
                {
                   Debug.LogError($"Point with ID:{servantData.PointId} not registered"); 
                   continue;
                }
                ServantController servant = _servantFactory.CreateServant(servantData, position);
                _servants.Add(servant);
            }
        }
    }
}