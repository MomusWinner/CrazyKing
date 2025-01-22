using System;
using UnityEngine;

namespace Servant.Archer.Upgrades
{
    [Serializable]
    public class ArcherParamData
    {
        public float speed;
        public int health;
        public int damage;
        public float attackSpeed;
        public float range;
        public RuntimeAnimatorController animatorController;
        public string arrowPath;
    }
}