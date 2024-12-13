using Controllers.UpgradeController;
using Servant.FSM;
using Servant.Knight.FSM;
using Servant.Knight.Upgrades;
using UnityEngine;
using VContainer;

namespace Servant.Knight
{
    public class KnightController : ServantController
    {
        public ServantFSM<KnightController> Fsm => _fsm;
        public float AttackRadius =>  _attackRadius;
        public int AttackDamage => _damage;
        public KnightUpgradeController _upgradeController;
            
        [Inject] private ServantFSM<KnightController> _fsm;

        [SerializeField] private float _attackRadius;
        [SerializeField] private int _damage;

        protected override void Start()
        {
            base.Start();
            _upgradeController = new KnightUpgradeController(this);
            _upgradeController.Setup();
            _fsm.SetUp(this);
            _fsm.ChangeState<KnightFollowToKingState>();
        }

        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                _upgradeController.Upgrade();
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                _upgradeController.Downgrade();
            }
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