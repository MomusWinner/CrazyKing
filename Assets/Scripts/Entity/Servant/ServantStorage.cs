using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using NUnit.Framework;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Entity.Servant
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
            if (servantData.ID == -1)
                servantData.ID = GetNextId();
            
            _servants.Add(servantData);
            Save();
            OnServantsUpdated?.Invoke(Servants);
            OnAddServant?.Invoke(servantData);
        }

        public ServantData CreateServantData(ServantType type, int lv = 1)
        {
            bool isUsed = _servants.Count < _servantsSO.MaxUsedServants;
            int pointId = -1;

            if (isUsed)
            {
                pointId = GetFreePointId();
                if (pointId == -1)
                {
                    Debug.LogError("Incorrect point id generation");
                }
            }

            var data = new ServantData()
            {
                ID = GetNextId(),
                IsUsed = isUsed,
                Lv = lv,
                Type = type,
                PointId = pointId,
            };
            
            return data;
        }

        public int GetNextId()
        { // TODO: IMPORTANT
            return _servants.Any() ? _servants.Last().ID + 1 : 0;
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
 
        public bool TryMergeServants(List<int> idsToMerge)
        {
            List<int> ids = new List<int>(idsToMerge);
            int upServantId = ids.First();
            ids.Remove(upServantId);
            foreach (var id in ids)
                RemoveServant(id);
            UpgradeServant(upServantId);
            return true;
        }

        public string GetInfo()
        {
            string result = string.Empty;
            
            foreach (var servant in _servants)
            {
                result += "---------------\n";
                result += servant.ToString();
            }

            return result;
        }

        private void Save()
        {
            _saveManager.Save();
        }

        private int GetServantsIndexById(int id)
        {
            return _servants.FindIndex(x => x.ID == id);
        }

        private int GetFreePointId()
        {
            if (_servants.Count >= _servantsSO.MaxUsedServants) return -1;

            if (!_servants.Any())
                return 0;
            
            var sortedServants = _servants.OrderByDescending(x => x.PointId).Reverse().ToArray();

            for (int i = 1; i < sortedServants.Count(); i++)
            {
                if (sortedServants[i].PointId - sortedServants[i - 1].PointId > 1)
                    return sortedServants[i].PointId + 1;
            }
            if (sortedServants.Last().PointId < _servantsSO.MaxUsedServants - 1)
                return sortedServants.Last().PointId + 1;
            
            return -1;
        }
    }

    [Serializable]
    public class ServantData
    {
        public int ID = -1;
        public int Lv = 1;
        public bool IsUsed;
        public ServantType Type;
        public int PointId;

        public override string ToString()
        {
            string result = string.Empty;
            result += "Type: " + Type + "\n";
            result += "Level: " + Lv + "\n";
            result += "IsUsed: " + IsUsed + "\n";
            result += "PointId: " + PointId + "\n";

            return result;

        }
    }
}