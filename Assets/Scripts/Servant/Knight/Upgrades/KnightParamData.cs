using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Servant.Knight.Upgrades
{
    [Serializable]
    public class KnightParamData
    {
        public int Health;
        public int Damage;
        public float Speed;
        public float AttackSpeed;
        public KnightSkinType KnightSkin;
    }
}