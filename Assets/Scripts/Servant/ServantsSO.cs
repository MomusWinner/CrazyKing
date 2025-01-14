using System.Collections.Generic;
using King;
using UnityEngine;

namespace Servant
{

    
    [CreateAssetMenu(menuName = "Game/Servant/ServantsSO")]
    public class ServantsSO : ScriptableObject
    {
        public int intervalOfEvolutionLevels = 5;
        public List<ServantSO> availableServants;
        public List<ServantsRing> rings;

        public ServantSO GetServantByType(ServantType type)
        {
            return availableServants.Find(x => x.type == type);
        }
    }
}