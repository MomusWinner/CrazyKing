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
        public abstract object StartValue { get; }
    }

    [Serializable]
    public class KingHealthParameter : KingParameter
    {
        public int startHealth;
        public List<HealthKingParameterUp> parametersUpgrades;
        public override List<KingParameterUp> Upgrades => parametersUpgrades.Cast<KingParameterUp>().ToList();
        public override object StartValue => startHealth;
    }
    
    [Serializable]
    public class KingDamageParameter: KingParameter
    {
        public int startDamage;
        public List<DamageKingParameterUp> parametersUpgrades;
        public override List<KingParameterUp> Upgrades => parametersUpgrades.Cast<KingParameterUp>().ToList();
        public override object StartValue => startDamage;
    }

    public enum KingParameterType
    {
        Health,
        Damage,
    }
}