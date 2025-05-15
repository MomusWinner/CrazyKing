using Agent;
using Entity.King;
using Entity.Servant;
using Entity.Servant.FSM;
using UnityEngine;

namespace Entity.States
{
    public class FollowToKingState : EntityState 
    {
        private int _isRunning = Animator.StringToHash("isRunning");
        private readonly KingController _king;
        private readonly ServantStatesSO _statesSo;
        private float _startSpeed;
        private EntityController _entity;
        private ServantController _servant;
        private PhysicAgent _physicAgent;

        public FollowToKingState(KingController king, ServantStatesSO servantStatesSO)
        {
            this._king = king;
            _statesSo = servantStatesSO;
        }
        
        public override void Start()
        {
            _servant = Entity.GetComponent<ServantController>();
            _physicAgent = Entity.GetComponent<PhysicAgent>();
            _startSpeed = _servant.Speed;
        }

        public override void Update()
        {
        }

        public override void FixedUpdate()
        {
            FollowToKnight();
        }

        public void FollowToKnight()
        {
            IPoint followingPoint = _servant.Point;
            if (followingPoint is null || !_king)
                return;
            Vector3 pointPos = followingPoint.GetPosition();
            Vector3 dirToPoint = pointPos - _servant.transform.position;
            float sqrMagnitude = dirToPoint.sqrMagnitude;
            if (sqrMagnitude < 0.3f)
            {
                _servant.Animator.SetBool(_isRunning, false);
                return;
            }
            float speedCof = _statesSo.speedByApproachingToTarget.Evaluate(sqrMagnitude);
            _servant.SetSpeed(speedCof * _startSpeed);
            _servant.Animator.SetBool(_isRunning, true);
            _physicAgent.Move(pointPos);
        }
        
        public override void Dispose()
        {
            _servant.SetSpeed(_startSpeed);
        }
    }
}