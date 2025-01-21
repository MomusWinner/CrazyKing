using System;
using UnityEditor.Animations;

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
        public AnimatorController animatorController;
        public string arrowPath;
    }
}