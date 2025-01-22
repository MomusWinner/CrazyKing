using System;
using UnityEngine;

namespace Servant.Knight.Upgrades
{
    [Serializable]
    public class KnightParamData
    {
        public int health;
        public int damage;
        public float attackSpeed;
        public float speed;
        public RuntimeAnimatorController animatorController;
    }
}