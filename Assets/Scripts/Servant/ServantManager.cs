using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Servant
{
    public enum ServantType
    {
        Knight
    }
    
    public class ServantManager: IStartable
    {
        public IList<ServantController> KingServants => _kingServants.AsReadOnly();
        public Action<IList<ServantController>> onKingServantsChanged;
        
        private ServantsSO _servantsSO;
        private readonly ServantFactory _servantFactory;
        private List<ServantSO> _servants;
        private List<ServantController> _kingServants = new(); 
        
        [Inject]
        public ServantManager(ServantsSO servantsSO, ServantFactory servantFactory)
        {
            _servants = new List<ServantSO>(servantsSO.availableServants);
            _servantsSO = servantsSO;
            _servantFactory = servantFactory;
        }
        
        public void Start()
        {
            Debug.Log("Start Servants Managers");
            Load();
        }

        public void AddServantAndSave(ServantController servant)
        {
            _kingServants.Add(servant);
            Save();
            onKingServantsChanged?.Invoke(KingServants);
        }


        public void RemoveServant()
        {
            
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(new ServantsSaveData()
            {
                servants = _kingServants.Select(s => ServantControllerToServantData(s)).ToList()
            });
            PlayerPrefs.SetString("servants", json);
        }

        public void Load()
        {
            string json = PlayerPrefs.GetString("servants");
            ServantsSaveData servants =  JsonConvert.DeserializeObject<ServantsSaveData>(json);
            if (servants is null)
            {
                ServantData firstServant = GetFirstServantData();
                ServantSO servantSO = ServantDataToServantSO(firstServant);
                ServantController servantController = _servantFactory.CreateServant(servantSO);
                _kingServants.Add(servantController);
                Save();
                return;
            }
            
            foreach (var servantData in servants.servants)
            {
                ServantController servantController = _servantFactory.CreateServant(ServantDataToServantSO(servantData));
                _kingServants.Add(servantController);
            }
        }

        public ServantSO ServantDataToServantSO(ServantData servantData)
        {
            ServantSO servantSO = _servantsSO.availableServants.Find(s => s.type == servantData.type);
            return servantSO;
        }

        public ServantData GetFirstServantData()
        {
            return new ServantData()
            {
                type = ServantType.Knight,
                lv = 1
            };
        }

        public ServantData ServantControllerToServantData(ServantController servantController)
        {
            return new ServantData()
            {
                type = servantController.ServantData.type,
                lv = servantController.UpgradeController.Level
            };
        }
    }

    public class ServantsSaveData
    {
        public List<ServantData> servants;
    }

    public class ServantData
    {
        public int lv;
        public ServantType type;
    }
}