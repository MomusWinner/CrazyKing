using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace King.Upgrades.Parameters
{
    [CreateAssetMenu(menuName = "Game/King/Parameters")]
    public class KingParametersSO : ScriptableObject
    {
        public KingHealthParameter HealthParameter;
        public KingDamageParameter DamageParameter;
        public KingServantAmountParameter ServantAmountParameter;

        public float AttackDistance = 0.5f;
        public float AttackRadius = 0.5f;
        public float JerkForce = 3000;

        public Dictionary<KingParameterType, KingParameter> GetKingParameters()
        {
            return new Dictionary<KingParameterType, KingParameter>
            {
                { KingParameterType.Health, HealthParameter },
                { KingParameterType.Damage, DamageParameter },
                { KingParameterType.ServantAmount, ServantAmountParameter }
            };
        }        
    }
}