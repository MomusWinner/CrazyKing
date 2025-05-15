using System;
using System.Collections;
using Controllers;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Entity.King.FSM
{
    public class KickState : KingState
    {
        private Type _nextStateType;
        private const string KickPrefPath = "King/Kick";
        private Kick _kick;
        private IEnumerator _rotationCoroutine;
        private Movement _movement;
        private FootController _footController;
        [Inject] private InputManager _inputManager;

        public override void Start()
        {
            _footController = King.GetComponentInChildren<FootController>();
            
            if (King == null)
                return;
            _movement = King.GetComponent<Movement>();
            _movement.FreezeMovement = true;
            _movement.FreezeRotation = true;
            
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

            King.StartCoroutine(Kick(attackDir));
        }

        public override void Dispose()
        {
            if (_movement == null) return;
            _movement.FreezeMovement = false;
            _movement.FreezeRotation = false;
            King.StopCoroutine(_rotationCoroutine);
        }

        public override void Message(string name, object obj = null)
        {
            _nextStateType = name switch
            {
                "next_state" => (Type)obj,
                _ => _nextStateType
            };
        }

        public IEnumerator Kick(Vector2 attackDir)
        {
            var kick = Resources.Load<GameObject>(KickPrefPath);
            _kick = Object.Instantiate(kick).GetComponent<Kick>();
            _kick.transform.position = King.transform.position;
            _kick.Setup(attackDir);
            _kick.transform.position += _kick.transform.right * (King.Radius);
            
            _footController.HideRightFoot();
            yield return new WaitForSeconds(0.3f);
            _footController.ShowRightFoot();
                
            if (King != null)
            {
                if (_nextStateType != null)
                    King.Fsm.ChangeState(_nextStateType);
                else
                    King.Fsm.ChangeState<DefaultKingState>();
            }
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
    }
}