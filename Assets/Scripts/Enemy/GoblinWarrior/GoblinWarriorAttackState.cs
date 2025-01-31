using Enemy.FSM;
using EntityBehaviour;
using UnityEngine;
using VContainer;

namespace Enemy.GoblinWarrior
{
    public class GoblinWarriorAttackState : EnemyState<GoblinWarriorController>
    {
        private readonly int _attack = Animator.StringToHash("Attack");
        private WarriorAttackBehaviour _warriorBehaviour;
        [Inject] private IObjectResolver _container;
        
        public override void Start()
        {
            base.Start();
            _warriorBehaviour = new WarriorAttackBehaviour(Enemy);
            _container.Inject(_warriorBehaviour);
            _warriorBehaviour.OnComplete += () => Enemy.Fsm.ChangeState<GoblinWarriorStayState>();
            _warriorBehaviour.OnStartAttack += () => Enemy.Animator.SetTrigger(_attack);
            _warriorBehaviour.Start();
        }

        public override void Update()
        {
            base.Update();
            _warriorBehaviour.Update(Time.deltaTime);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            _warriorBehaviour.FixedUpdate(Time.deltaTime);
        }

        public override void Message(string name, object obj)
        {
            switch (name)
            {
                case "attack":
                    _warriorBehaviour.Attack();
                    break;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _warriorBehaviour.Dispose();
        }
    }
}