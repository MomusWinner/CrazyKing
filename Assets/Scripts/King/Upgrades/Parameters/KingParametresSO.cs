using System;
using System.Collections.Generic;
using System.Linq;

namespace King.Upgrades.Parameters
{
    [Serializable]
    public abstract class KingParameter
    {
        public string name;
        public KingParameterType type;
        public abstract List<KingParameterUp> Upgrades {get; }
    }

    [Serializable]
    public class KingHealthParameter : KingParameter
    {
        public List<HealthKingParameterUp> parametersUpgrades;
        public override List<KingParameterUp> Upgrades => parametersUpgrades.Cast<KingParameterUp>().ToList();
    }
    
    [Serializable]
    public class KingDamageParameter: KingParameter
    {
        public List<DamageKingParameterUp> parametersUpgrades;
        public override List<KingParameterUp> Upgrades => parametersUpgrades.Cast<KingParameterUp>().ToList(); 
    }

    public enum KingParameterType
    {
        Health,
        Damage,
        ServantAmout,
    }
}