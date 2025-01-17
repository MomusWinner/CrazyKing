using King;
using UnityEngine;

namespace Servant.FSM
{
    public abstract class FollowKingState<TServant>: ServantState<TServant> where TServant:ServantController
    {
        protected abstract int isRun { get; }
        protected readonly KingController king;
        private readonly ServantStatesSO _statesSo;
        protected IPoint followingPoint;
        private float _startSpeed;
        

        public FollowKingState(KingController king, ServantStatesSO statesSO)
        {
            this.king = king;
            _statesSo = statesSO;
        }
        
        public override void Start()
        {
            followingPoint = Servant.Point;
            _startSpeed = Servant.Speed;
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
            if (followingPoint is null || !king)
                return;
            Vector3 pointPos = followingPoint.GetPosition();
            Vector3 dirToPoint = pointPos - Servant.transform.position;
            float sqrMagnitude = dirToPoint.sqrMagnitude;
            if (sqrMagnitude < 0.3f)
            {
                Servant.Animator.SetBool(isRun, false);
                return;
            }
            float speedCof = _statesSo.speedByApproachingToTarget.Evaluate(sqrMagnitude);
            Servant.SetSpeed(speedCof * _startSpeed);
            Servant.Animator.SetBool(isRun, true);
            Servant.SetDestination(pointPos);
        }
        
        public override void Dispose()
        {
            Servant.SetSpeed(_startSpeed);
        }
    }
}