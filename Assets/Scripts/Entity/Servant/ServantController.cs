using System.Collections;
using Entity.King;
using Entity.Servant.FSM;
using Entity.States;
using Servant;
using UnityEngine;
using VContainer;

namespace Entity.Servant
{
    public abstract class ServantController : EntityController, IThrowable
    {
        public int ID { get; private set; }
        public ServantSO ServantSO { get; set; }
        public ServantData ServantData { get; set; }
        public IPoint Point { get; set; }
        
        [HideInInspector]
        [Inject] public KingController KingController;
        [Inject] public ServantFSM FSM;
        [Inject] private IObjectResolver _container;

        [SerializeField] private EntityStateType _attackState;
        [SerializeField] private EntityStateType _defaultState = EntityStateType.FollowToKing;

        public override void Initialize()
        {
            base.Initialize();
            if (ServantData.IsUsed)
            {
                if (KingController.TryGetPoint(ServantData.PointId, out IPoint point)) Point = point;
                else Debug.LogError($"ServantPoint ID:{ServantData.PointId} already busy.");
            }
            else
            {
                if (KingController.TryGetFreePoint(out IPoint point)) Point = point;
                else if (ServantData.IsUsed) Debug.LogError($"ServantPoint ID:{ServantData.PointId} already busy.");
            }
            ID = ServantData.ID;
            KingController.AddServant(this);
            TargetFinder = new EntityFinder(LookRadius,
                targetMask: LayerMasks.Enemy,
                ignoreMask: LayerMasks.King | LayerMasks.LowBarrier | LayerMasks.Default);
        }

        protected override void Start()
        {
            FSM.Setup(this, _attackState, _defaultState);
        }

        protected override void Update()
        {
            base.Update();
            FSM.Update();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            FSM.FixedUpdate();
        }

        public abstract void StartFirstState();
        
        public override void OnDead()
        {
            if (Point is not null)
                KingController.ReturnPoint(Point);
            KingController.RemoveServant(this);
            base.OnDead();
        }

        public void Throw(Vector2 direction, float force)
        {
            RigidBody.AddForce(direction * force, ForceMode2D.Impulse);
            StartCoroutine(Wakeup());
        }

        private IEnumerator Wakeup()
        {
            yield return new WaitForSeconds(1f);
        }
    }
}