using System;
using BaseEntity;
using BaseEntity.States;
using UnityEngine;

namespace Enemy.AngryDog
{
    public class AngryDogController : EnemyController, IWarrior
    {
        public Action OnAttack { get; set; }
        public string AttackSound => _attackSound;
        public EntityController Controller => this;
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