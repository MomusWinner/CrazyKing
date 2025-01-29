using System.Collections;
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
        private IEnumerator rotationCorutine;

        public override void Start()
        {
            if (King == null) return;
            _enemyMask = LayerMask.GetMask("Enemy");
            King.Animator.SetTrigger(Attack);
            _movement = King.GetComponent<Movement>();
            _movement.FreezeRotation = true;
            _movement.FreezeMovement = true;
            
            Vector2 attackDir = _inputManager.MoveDirection;
            
            if (_inputManager.MouseAvailable)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(_inputManager.MousePosition);
                Vector2 kingPos = King.transform.position;
                attackDir = mousePos - kingPos;
                attackDir.Normalize();
                float angle = attackDir.ToAngle();
                rotationCorutine = Rotate(angle);
                King.StartCoroutine(rotationCorutine);
            }
            
            King.RigidBody.AddForce(attackDir * _parameters.jerkForce);
        }

        public IEnumerator Rotate(float angle)
        {
            while (true)
            {
                var rotation = Quaternion.Lerp(
                    King.transform.rotation,
                    Quaternion.Euler(new Vector3(0f,0f,angle)),
                    20f*Time.fixedDeltaTime);
                King.RigidBody.MoveRotation(rotation);
                yield return new WaitForFixedUpdate();
            }
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
            if(King == null) return; 
            
            King.StopCoroutine(rotationCorutine);
            _movement.FreezeRotation = false;
            _movement.FreezeMovement = false;
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