using System;
using BaseEntity;
using BaseEntity.States;
using Enemy.FSM;
using EntityBehaviour;
using UnityEngine;
using VContainer;

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

        //public EnemyFSM<GoblinWarriorController> Fsm => _fsm;
        

        protected override void Start()
        {
            
            base.Start();
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
           // _fsm?.SendMessage("attack");
        }

        protected new void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRadius);
        }
    }
}