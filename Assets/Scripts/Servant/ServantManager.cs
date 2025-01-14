using System.Collections.Generic;
using King;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Servant
{
    public class ServantManager : IStartable
    {
        public IReadOnlyCollection<ServantController> Servants => _servants.AsReadOnly(); 
        [Inject] private ServantFactory _servantFactory;
        [Inject] private ServantStorage _servantStorage;
        [Inject] private KingController _king;
        
        private readonly List<ServantController> _servants = new ();
        
        public void Start()
        {
            LoadKingServants();
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
                servant.Init();
            }
        }
    }
}