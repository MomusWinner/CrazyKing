using System.Collections;
using Controllers;
using UnityEngine;
using VContainer;

namespace King.FSM
{
    public class KickState : KingState
    {
        private const string KickPrefPath = "King/Kick";
        private Kick _kick;
        private IEnumerator _rotationCoroutine;
        [Inject] private InputManager _inputManager;

        public override void Start()
        {
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
            var kick = Resources.Load<GameObject>(KickPrefPath);
            _kick = Object.Instantiate(kick).GetComponent<Kick>();
            _kick.transform.position = King.transform.position;
            _kick.Setup(attackDir);
            King.Fsm.ChangeState<DefaultKingState>();
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