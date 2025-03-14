using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using VContainer;
using VContainer.Unity;

namespace Servant
{
    public class ServantStorage : IStartable
    {
        public Action<ServantData> OnAddServant;
        public Action<int> OnRemoveServant;
        public Action<ServantData> OnUpgradeServant;
        /// <summary>
        /// On add, remove and update servant
        /// </summary>
        public Action<IReadOnlyCollection<ServantData>> OnServantsUpdated { get; set; }
        public IReadOnlyCollection<ServantData> Servants => _servants.AsReadOnly();
        
        private List<ServantData> _servants;
        [Inject] private ServantsSO _servantsSO;
        [Inject] private SaveManager _saveManager; 
        
        public void Start()
        {
            _servants = _saveManager.GameData.Servants;
        }

        public void RemoveServant(int id)
        {
            int index = GetServantsIndexById(id);
            if (index == -1) return;
            _servants.RemoveAt(index);
            OnServantsUpdated?.Invoke(Servants);
            OnRemoveServant?.Invoke(id);
            Save();
        }
        
        public void AddServant(ServantData servantData)
        {
            servantData.ID = _servants.Any() ? _servants.Last().ID + 1 : 0;
            _servants.Add(servantData);
            Save();
            OnServantsUpdated?.Invoke(Servants);
            OnAddServant?.Invoke(servantData);
        }

        public void UpgradeServant(int id)
        {
            int index = GetServantsIndexById(id);
            if (index == -1) return;
            ServantData servantData = _servants[index];
            int maxUpgradeLv = _servantsSO.GetServantByType(servantData.Type).Upgrades.Count;
            if (servantData.Lv >= maxUpgradeLv) return;
            servantData.Lv++;
            Save();
            OnServantsUpdated?.Invoke(Servants);
            OnUpgradeServant?.Invoke(servantData);
        }

        public void UnsetServantPoint(int id)
        {
            int index = GetServantsIndexById(id);
            if (index == -1) return;
            ServantData servantData = _servants[index];
            servantData.IsUsed = false;
            Save();
        }

        public void SetServantPoint(int id, int pointId)
        {
            int index = GetServantsIndexById(id);
            if (index == -1) return;
            ServantData servantData = _servants[index];
            servantData.PointId = pointId;
            servantData.IsUsed = true;
            Save();
        }
 
        public bool TryMergeServants(List<int> ids)
        {
            int upServantId = ids.First();
            ids.Remove(upServantId);
            foreach (var id in ids)
                RemoveServant(id);
            UpgradeServant(upServantId);
            return true;
        }       
        
        private void Save()
        {
            _saveManager.Save();
        }

        private int GetServantsIndexById(int id)
        {
            return _servants.FindIndex(x => x.ID == id);
        }
    }

    [Serializable]
    public class ServantData
    {
        public int ID;
        public int Lv = 1;
        public bool IsUsed;
        public ServantType Type;
        public int PointId;
    }
}