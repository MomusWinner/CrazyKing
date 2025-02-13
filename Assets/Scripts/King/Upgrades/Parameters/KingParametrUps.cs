using System;

namespace King.Upgrades.Parameters
{
    [Serializable]
    public abstract class KingParameterUp
    {
        public string description;
        public int price;
        public abstract object Value { get; }
    }

    [Serializable]
    public class HealthKingParameterUp : KingParameterUp 
    {
        public int health;
        public override object Value => health; 
    }

    [Serializable]
    public class DamageKingParameterUp : KingParameterUp
    {
        public int damage;
        public override object Value => damage;
    }

    [Serializable]
    public class ServantAmountParameterUp : KingParameterUp
    {
        public int servantAmount;
        public override object Value => servantAmount;
    }
}