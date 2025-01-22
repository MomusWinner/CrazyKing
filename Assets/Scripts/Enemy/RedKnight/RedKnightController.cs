using Enemy.FSM;
using UnityEngine;
using VContainer;

namespace Enemy.RedKnight
{
    public class RedKnightController: EnemyController
    {
        public int AttackDamage => _attackDamage;
        public float AttackRadius => _attackRadius;
        
        [SerializeField] private int _attackDamage;
        [SerializeField] private float _attackRadius;
        
        public EnemyFSM<RedKnightController> Fsm => _fsm;
        
        [Inject]
        private EnemyFSM<RedKnightController> _fsm;
        
        protected override void Start()
        {
            base.Start();
            _fsm.Setup(this);
            _fsm.ChangeState<RedKnightStayState>();
        }

        protected override void Update()
        {
            base.Update();
            _fsm?.Update();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            _fsm?.FixedUpdate();
        }

        public void Attack()
        {
            _fsm?.SendMessage("attack");
        }
        
        protected new void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRadius + Radius);
        }
    }
}