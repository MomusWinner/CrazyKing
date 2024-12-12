using King;
using UnityEngine;

namespace Servant.FSM
{
    public abstract class FollowKingState<TServant>: ServantState<TServant> where TServant:ServantController
    {
        protected abstract int isRun { get; }
        protected readonly KingController king;
        protected IPoint followingPoint;
        

        public FollowKingState(KingController king)
        {
            this.king = king;
        }
        
        public override void Start()
        {
            followingPoint = king.GetFreePoint();
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
            
            Servant.Animator.SetBool(isRun, true);
            Servant.SetDestination(pointPos);
        }
        
        public override void Dispose()
        {
        }
    }
}