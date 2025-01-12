using System.Collections.Generic;
using UnityEngine;

namespace Servant
{
    [CreateAssetMenu(menuName = "Game/Servant/ServantsSO")]
    public class ServantsSO : ScriptableObject
    {
        public int maxServants = 10;
        public int intervalOfEvolutionLevels = 5;
        public List<ServantSO> availableServants;

        public ServantSO GetServantByType(ServantType type)
        {
            return availableServants.Find(x => x.type == type);
        }
    }
}