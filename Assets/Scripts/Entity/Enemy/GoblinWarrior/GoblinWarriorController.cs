using System;
using Entity.States;
using UnityEngine;

namespace Entity.Enemy.GoblinWarrior
{
    public class GoblinWarriorController : EnemyController, IWarrior
    {
        public Action OnAttack { get; set; }
        public EntityController Controller => this;
        public string AttackSound => _attackSound;
        public int AttackDamage => attackDamage;
        public float AttackRadius => Radius + attackRadius; 

        [SerializeField] private int attackDamage;
        [SerializeField] private float attackRadius;
        [SerializeField] private string _attackSound;

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