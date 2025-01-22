using System;
using BaseEntity;
using Enemy.FSM;
using EntityBehaviour;
using UnityEngine;
using VContainer;

namespace Enemy.GoblinWarrior
{
    public class GoblinWarriorController : EnemyController, IWarrior
    {
        public EntityController Controller => this;
        public int AttackDamage => attackDamage;
        public float AttackRadius => Radius + attackRadius; 

        [SerializeField] private int attackDamage;
        [SerializeField] private float attackRadius;

        public EnemyFSM<GoblinWarriorController> Fsm => _fsm;
        
        [Inject] private EnemyFSM<GoblinWarriorController> _fsm;

        protected override void Start()
        {
            
            base.Start();
            _fsm.Setup(this);
            _fsm.ChangeState<GoblinWarriorStayState>();
        }

        protected override void Update()
        {
            base.Update();
            _fsm.Update();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            _fsm.FixedUpdate();
        }

        public void Attack()
        {
            _fsm?.SendMessage("attack");
        }

        protected new void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRadius);
        }
    }
}