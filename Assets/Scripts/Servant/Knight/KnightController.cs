using Servant.FSM;
using Servant.Knight.FSM;
using UnityEngine;
using VContainer;

namespace Servant.Knight
{
    public class KnightController : ServantController
    {
        public ServantFSM<KnightController> Fsm => _fsm;
        
        public float AttackRadius =>  _attackRadius + Radius;
        public int AttackDamage { get; set; }
        
        [Inject] private ServantFSM<KnightController> _fsm;

        [SerializeField] private float _attackRadius;

        public override void Initialize()
        {
            _fsm.SetUp(this);
            base.Initialize();
        }

        public override void StartFirstState()
        {
            _fsm.ChangeState<KnightFollowToKingState>();
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
            _fsm?.SendMessage("attack", null);
        }
        
        protected new void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Radius + AttackRadius);
        }
    }
}