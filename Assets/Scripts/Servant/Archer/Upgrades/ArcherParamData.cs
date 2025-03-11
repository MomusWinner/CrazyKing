using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Servant.Archer.Upgrades
{
    [Serializable]
    public class ArcherParamData
    {
        public ArcherSkinType SkinType;
        [ServantParameter]
        public float Speed;
        public int Health;
        public int Damage;
        public string ArrowPath;
    }

    public class ServantParameterAttribute : Attribute
    {
        public ServantParameterAttribute()
        {
            
        }
        
    }
}