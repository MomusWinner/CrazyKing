using System;

namespace Entity.Servant.Archer.Upgrades
{
    [Serializable]
    public class ArcherParamData
    {
        public ArcherSkinType SkinType;
        public float Speed;
        public int Health;
        public int Damage;
        public float LookRadius;
        public string ArrowPath;
    }
}