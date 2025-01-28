using System.Collections.Generic;
using UnityEngine;

namespace King.Upgrades.Parameters
{
    [CreateAssetMenu(menuName = "Game/King/Parameters")]
    public class KingParametersSO : ScriptableObject
    {
        public KingHealthParameter healthParameter;
        public KingDamageParameter damageParameter;

        public float attackDistance;
        public float attackRadius;
        public float jerkForce;

        public Dictionary<KingParameterType, KingParameter> GetKingParameters()
        {
            return new Dictionary<KingParameterType, KingParameter>
            {
                { KingParameterType.Health, healthParameter },
                { KingParameterType.Damage, damageParameter },
            };
        }        
    }
}