using System;
using BaseEntity;
using BaseEntity.States;
using EntityBehaviour;
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

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void StartFirstState()
        {
        }
        
        protected override void Update()
        {
            base.Update();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
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