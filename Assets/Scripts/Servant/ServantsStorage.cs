using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Servant
{
    public class ServantsStorage : IStartable
    {
        /// <summary>
        /// On add or remove servant
        /// </summary>
        public Action<IList<ServantData>> OnServantsUpdated { get; set; }
        public IList<ServantData> Servants => _servants.AsReadOnly();
        
        private List<ServantData> _servants;
        [Inject] private ServantsSO _servantsSO;
        
        public void Start()
        {
            Load();
        }

        public void RemoveServant(int id)
        {
            int index = GetServantsIndexById(id);
            if (index == -1) return;
            Servants.RemoveAt(index);
            OnServantsUpdated?.Invoke(Servants);
        }
        
        public void AddServant(ServantData servantData)
        {
            servantData.ID = _servants.Any() ? _servants.Last().ID + 1 : 0;
            _servants.Add(servantData);
            Save();
            OnServantsUpdated?.Invoke(Servants);
        }

        public void UpgradeServant(int id)
        {
            int index = GetServantsIndexById(id);
            if (index == -1) return;
            ServantData servantData = _servants[index];
            int maxUpgradeLv = _servantsSO.GetServantByType(servantData.Type).upgrades.Count;
            if (servantData.Lv >= maxUpgradeLv) return;
            servantData.Lv++;
            Save();
        }

        private void Load()
        {
            string servantsJson = PlayerPrefs.GetString("Servants");
            _servants = JsonConvert.DeserializeObject<List<ServantData>>(servantsJson) ?? new List<ServantData>();
        }

        private void Save()
        {
            string servantsJson = JsonConvert.SerializeObject(_servants);
            PlayerPrefs.SetString("Servants", servantsJson);
            Debug.Log(servantsJson);
        }

        private int GetServantsIndexById(int id)
        {
            return _servants.FindIndex(x => x.ID == id);
        }
    }

    public class ServantData
    {
        public int ID { get; set; }
        public int Lv { get; set; }
        public bool IsUsed { get; set; }
        public ServantType Type { get; set; }
        public int? PointNumber { get; set; }
    }
}