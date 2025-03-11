using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Servant.Archer.Upgrades
{
    [Serializable]
    public class ArcherParamData
    {
        public ArcherSkinType SkinType;
        public float Speed;
        public int Health;
        public int Damage;
        public int LookRadius;
        public string ArrowPath;
    }
}