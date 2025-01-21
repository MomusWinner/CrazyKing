using Servant.Archer.FSM;
using Servant.FSM;
using VContainer;

namespace Servant.Archer
{
    public class ArcherController : ServantController
    {
        public ServantFSM<ArcherController> Fsm => _fsm;
        public string ArrowPrefPath { get; set; }
        public int AttackDamage { get; set; }
        
        [Inject] private ServantFSM<ArcherController> _fsm;

        public override void Initialize()
        {
            _fsm.SetUp(this);
            base.Initialize();
        }

        public override void StartFirstState()
        {
            _fsm.ChangeState<ArcherFollowToKingState>();
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
            
        }
    }
}