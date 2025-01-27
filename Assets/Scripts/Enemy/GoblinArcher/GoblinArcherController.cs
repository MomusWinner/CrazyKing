using BaseEntity;
using Enemy.FSM;
using Enemy.GoblinWarrior;
using UnityEngine;
using VContainer;

namespace Enemy.GoblinArcher
{
    public class GoblinArcherController : EnemyController
    {
        public EnemyFSM<GoblinArcherController> Fsm => _fsm;
        public EntityController Controller => this;
        public int AttackDamage => attackDamage;

        [SerializeField] private int attackDamage;
        public string ArrowPrefPath;
        
        [Inject] private EnemyFSM<GoblinArcherController> _fsm;


        protected override void Start()
        {
            base.Start();
            _fsm.Setup(this);
            _fsm.ChangeState<GoblinArcherStayState>();
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
    }
}