using System;

namespace King.Upgrades.Parameters
{
    [Serializable]
    public abstract class KingParameterUp
    {
        public string description;
        public int price;
    }

    [Serializable]
    public class HealthKingParameterUp : KingParameterUp 
    {
        public int health;
    }

    [Serializable]
    public class DamageKingParameterUp : KingParameterUp
    {
        public int damage;
    }
}