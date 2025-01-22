using Controllers;
using Enemy;
using King.Upgrades.Parameters;
using UnityEngine;
using VContainer;

namespace King.FSM
{
    public class KingAttackState : KingState
    {
        [Inject] InputManager _inputManager;
        [Inject] KingParametersSO _parameters;
        private static readonly int Attack = Animator.StringToHash("Attack");
        private Movement _movement;
        private LayerMask _enemyMask;

        public override void Start()
        {
            _enemyMask = LayerMask.GetMask("Enemy");
            King.Animator.SetTrigger(Attack);
            _movement = King.GetComponent<Movement>();
            if (_inputManager.MouseAvailable)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(_inputManager.MousePosition);
                Vector2 kingPos = King.transform.position;
                King.transform.rotation = Quaternion.Euler(new Vector3(0,0, (mousePos - kingPos).ToAngle()));
            }
            _movement.FreezeRotation = true;
        }

        public override void Message(string name, object obj)
        {
            base.Message(name, obj);
            if (name == "set_damage")
            {
                SetDamage();    
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _movement.FreezeRotation = false;
        }

        private void SetDamage()
        {
            var targets = Physics2D.OverlapCircleAll(
                King.transform.position + _parameters.attackDistance * King.transform.right, _parameters.attackRadius, _enemyMask);
            foreach (var target in targets)
            {
                var enemy = target.GetComponent<EnemyController>();
                if (enemy != null)
                    enemy.Damage(King.AttackDamage);
            }
            King.Fsm.ChangeState<DefaultKingState>();
        }
    }
}