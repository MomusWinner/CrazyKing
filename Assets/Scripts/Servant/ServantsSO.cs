using System;
using System.Collections.Generic;
using King;
using UnityEngine;

namespace Servant
{
    [Serializable]
    public class ServantParameterSO
    {
        public string FriendlyName;
        public string FieldName;
        public Sprite Icon;
    }
    
    [CreateAssetMenu(menuName = "Game/Servant/ServantsSO")]
    public class ServantsSO : ScriptableObject
    {
        public int IntervalOfEvolutionLevels = 5;
        public List<ServantSO> AvailableServants;
        public List<ServantsRing> Rings;
        public List<ServantParameterSO> Parameters;

        public ServantSO GetServantByType(ServantType type)
        {
            return AvailableServants.Find(x => x.type == type);
        }
    }
}