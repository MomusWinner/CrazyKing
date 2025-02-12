using System;
using BaseEntity;
using BaseEntity.States;
using UnityEngine;

namespace Enemy.GoblinWarrior
{
    public class GoblinWarriorController : EnemyController, IWarrior
    {
        public Action OnAttack { get; set; }
        public EntityController Controller => this;
        public int AttackDamage => attackDamage;
        public float AttackRadius => Radius + attackRadius; 

        [SerializeField] private int attackDamage;
        [SerializeField] private float attackRadius;

        public void Attack()
        {
            OnAttack?.Invoke();
        }

        protected new void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRadius);
        }
    }
}