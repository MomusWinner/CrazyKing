using System;
using System.Collections.Generic;
using King.Upgrades.Parameters;
using Newtonsoft.Json;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace King
{
    public class KingParameterManager : IStartable
    {
        [Inject] private KingParametersSO _kingParameters;
        private Dictionary<KingParameterType, KingParameterData> _parameters = new();

        public void Start()
        {
            LoadParameters(); 
        }

        public int GetParameterLevel(KingParameterType type)
        {
            return _parameters[type].Lv;
        }
        
        public T GetParameterValue<T>(KingParameterType type)
        {
            if (_parameters.TryGetValue(type, out var result))
                return (T)result.Value;
            return default;
        }

        public void SetParameter(KingParameterData data)
        {
            _parameters[data.Type] = data;
            Save();
        }

        public void UpgradeParameter(KingParameterType type)
        {
            KingParameter parameter = _kingParameters.GetKingParameters()[type];
            if (!_parameters.TryGetValue(type, out var result) 
                || result.Lv >= parameter.Upgrades.Count) return;
            
           result.Value = parameter.Upgrades[result.Lv];
           result.Lv++; 
           Save();
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(_parameters);
            PlayerPrefs.SetString("king_parameters", json);
        }

        public void LoadParameters()
        {
            string json = PlayerPrefs.GetString("king_parameters");
            _parameters = JsonConvert.DeserializeObject<Dictionary<KingParameterType, KingParameterData>>(json);
            if (_parameters is null)
            {
                _parameters = new Dictionary<KingParameterType, KingParameterData>();
                foreach (var type in Enum.GetValues(typeof(KingParameterType)))
                {
                    _parameters.Add((KingParameterType)type, new KingParameterData());
                }
            }
        }
    }

    public class KingParameterData
    {
        public int Lv { get; set; }
        public KingParameterType Type { get; set; }
        public object Value { get; set; }
    }
}