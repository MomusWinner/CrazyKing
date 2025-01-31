using EntityBehaviour;
using Servant.FSM;
using UnityEngine;
using VContainer;

namespace Servant.Knight.FSM
{
    public class KnightAttackState : ServantState<KnightController>
    {
        private int _attack = Animator.StringToHash("Attack");
        private WarriorAttackBehaviour _warriorBehaviour;
        [Inject] private IObjectResolver _container;
        
        public override void Start()
        {
            _warriorBehaviour = new WarriorAttackBehaviour(Servant);
            _container.Inject(_warriorBehaviour);
            _warriorBehaviour.OnComplete += () => Servant.Fsm.ChangeState<KnightFollowToKingState>();
            _warriorBehaviour.OnStartAttack += StartAttackAnim;
            _warriorBehaviour.Start();
        }

        public override void Update()
        {
            _warriorBehaviour.Update(Time.deltaTime);
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
            _warriorBehaviour.Dispose();
        }
        
        public override void FixedUpdate()
        {
            _warriorBehaviour.FixedUpdate(Time.fixedDeltaTime);
        }

        private void StartAttackAnim()
        {
            Servant.Animator.SetTrigger(_attack);
        }
    }
}