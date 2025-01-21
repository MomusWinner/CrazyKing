using EntityBehaviour;
using Servant.FSM;
using UnityEngine;

namespace Servant.Archer.FSM
{
    public class ArcherAttackState : ServantState<ArcherController>
    {
        private ArcherAttackBehaviour _attackBehaviour;
        private readonly int _aimAnim = Animator.StringToHash("isAiming");
        
        public override void Start()
        {
            ArrowData arrowData = new ArrowData
            {
                damage = Servant.AttackDamage,
                speed = 8,
                distance = 10000,
                targetLayer = LayerMask.GetMask("Enemy")            
            };
            
            _attackBehaviour = new ArcherAttackBehaviour(Servant, Servant.ArrowPrefPath, 1f, arrowData);

            base.Start();
            _attackBehaviour.Start();
            _attackBehaviour.OnAim += () => Servant.Animator.SetBool(_aimAnim, true);
            _attackBehaviour.OnAttackCompleted += () => Servant.Fsm.ChangeState<ArcherFollowToKingState>();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            _attackBehaviour.FixedUpdate(Time.fixedDeltaTime);
        }

        public override void Update()
        {
            base.Update();
            _attackBehaviour.Update(Time.deltaTime);
        }

        public override void Dispose()
        {
            base.Dispose();
            Servant.Animator.SetBool(_aimAnim, false);
        }
    }
}