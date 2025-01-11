using System.Collections.Generic;
using UnityEngine;

namespace Servant
{
    [CreateAssetMenu(menuName = "Game/Servant/ServantsSO")]
    public class ServantsSO : ScriptableObject
    {
        public int maxServants = 10;
        public List<ServantSO> availableServants;
    }
}