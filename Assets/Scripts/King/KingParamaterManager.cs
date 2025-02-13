using System;
using System.Collections.Generic;
using Controllers;
using King.Upgrades.Parameters;
using VContainer;
using VContainer.Unity;

namespace King
{
    public class KingParameterManager : IStartable
    {
        public Action<KingParameterType> OnUpdater;
        [Inject] private KingParametersSO _kingParameters;
        [Inject] private SaveManager _saveManager;
        
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
            {
                if (result.Lv == 0)
                    return (T)_kingParameters.GetKingParameters()[type].StartValue;
                return (T)_kingParameters.GetKingParameters()[type].Upgrades[result.Lv - 1].Value;
            }
            return default;
        }

        public void UpgradeParameter(KingParameterType type)
        {
            KingParameter parameter = _kingParameters.GetKingParameters()[type];
            if (!_parameters.TryGetValue(type, out var result) 
                || result.Lv >= parameter.Upgrades.Count) return;
            
            result.Lv++; 
            OnUpdater?.Invoke(type);
            Save();
        }

        public void Save()
        {
            _saveManager.Save();
        }

        public void LoadParameters()
        {
            _parameters = _saveManager.GameData.KingParameters;
            if (_parameters.Count <= 0)
            {
                foreach (var type in Enum.GetValues(typeof(KingParameterType)))
                {
                    KingParameterData data = new KingParameterData();
                    KingParameterType kingType = (KingParameterType)type;
                    data.Type = kingType;
                    _parameters.Add(kingType, data);
                }
            }
        }
    }

    public class KingParameterData
    {
        public int Lv { get; set; }
        public KingParameterType Type { get; set; }
    }
}