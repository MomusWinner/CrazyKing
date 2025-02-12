using System;
using BaseEntity;
using BaseEntity.States;
using UnityEngine;

namespace Servant.Knight
{
    public class KnightController : ServantController, IWarrior
    {
        public Action OnAttack { get; set; }
        public EntityController Controller => this;
        public float AttackRadius =>  _attackRadius + Radius;
        public int AttackDamage { get; set; }

        [SerializeField] private float _attackRadius;

        public override void StartFirstState()
        {
        }
        
        public void Attack()
        {
            OnAttack?.Invoke();
        }
        
        protected new void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Radius + AttackRadius);
        }
    }
}