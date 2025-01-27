using Enemy.FSM;
using EntityBehaviour;
using UnityEngine;

namespace Enemy.GoblinArcher
{
    public class GoblinArcherAttackState : EnemyState<GoblinArcherController>
    {
          private ArcherAttackBehaviour _attackBehaviour;
        private readonly int _aimAnim = Animator.StringToHash("isAiming");
        
        public override void Start()
        {
            ArrowData arrowData = new ArrowData
            {
                damage = Enemy.AttackDamage,
                speed = 8,
                distance = 10000,
                targetLayer = LayerMask.GetMask("King")            
            };
            
            _attackBehaviour = new ArcherAttackBehaviour(Enemy, Enemy.ArrowPrefPath, 1f, arrowData);

            base.Start();
            _attackBehaviour.Start();
            _attackBehaviour.OnAim += () => Enemy.Animator.SetBool(_aimAnim, true);
            _attackBehaviour.OnAttackCompleted += () => Enemy.Fsm.ChangeState<GoblinArcherStayState>();
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
            _attackBehaviour.Dispose();
            Enemy.Animator.SetBool(_aimAnim, false);
        }
    }
}