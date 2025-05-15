using System;
using System.Collections;
using Controllers;
using Controllers.SoundManager;
using Cysharp.Threading.Tasks;
using Entity.King.Upgrades.Parameters;
using Health;
using UnityEngine;
using VContainer;

namespace Entity.King.FSM
{
    public class KingAttackState : KingState
    {
        [Inject] private InputManager _inputManager;
        [Inject] private KingParametersSO _parameters;
        [Inject] private SoundManager _soundManager;
        private Type _nextState;
        private static readonly int Attack = Animator.StringToHash("Attack");
        private Movement _movement;
        private IEnumerator _rotationCoroutine;

        public override void Start()
        {
            if (King == null) return;
            _soundManager.StartMusic("KingAttack", SoundChannel.Effect);
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
                _rotationCoroutine = Rotate(angle);
                King.StartCoroutine(_rotationCoroutine);
            }
            
            King.RigidBody.AddForce(attackDir * _parameters.JerkForce);
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

        public override async void Message(string name, object obj)
        {
            switch (name)
            {
                case "set_damage":
                    await SetDamage();
                    break;
                case "next_state":
                    _nextState = (Type)obj;
                    break;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            if(King == null) return; 
            
            King.StopCoroutine(_rotationCoroutine);
            _movement.FreezeRotation = false;
            _movement.FreezeMovement = false;
            _nextState = null;
        }

        private async UniTask SetDamage()
        {
            var targets = Physics2D.OverlapCircleAll(
                King.transform.position + _parameters.AttackDistance * King.transform.right,
                _parameters.AttackRadius,
                LayerMasks.Enemy | LayerMasks.NeutralEntity);
            foreach (var target in targets)
            {
                var enemy = target.GetComponent<IDamageable>();
                if (enemy != null)
                    enemy.Damage(King.AttackDamage);
            }
            await UniTask.WaitForSeconds(0.1f);
            if (King == null) return;
            if (_nextState != null)
                King.Fsm.ChangeState(_nextState);
            else
                King.Fsm.ChangeState<DefaultKingState>();
        }
    }
}