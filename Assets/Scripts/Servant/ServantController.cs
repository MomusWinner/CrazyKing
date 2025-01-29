using BaseEntity;
using Finders;
using King;
using UnityEngine;
using VContainer;

namespace Servant
{
    public abstract class ServantController : EntityController
    {
        public int ID { get; private set; }
        public ServantSO ServantSO { get; set; }
        public ServantData ServantData { get; set; }
        public IPoint Point { get; set; }
        
        [HideInInspector]
        [Inject]
        public KingController KingController;

        private LayerMask _enemyMask;
        private LayerMask _servantMask;

        public override void Initialize()
        {
            base.Initialize();
            if (KingController.TryGetPoint(ServantData.PointId, out IPoint point)) Point = point;
            else Debug.LogError($"ServantPoint ID:{ServantData.PointId} already busy.");
            ID = ServantData.ID;
            KingController.AddServant(this);
            _enemyMask = LayerMask.GetMask("Enemy");
            _servantMask = LayerMask.GetMask("King");
            LayerMask lowBarrier = LayerMask.GetMask("LowBarrier");
            LayerMask defaultLayerMask = LayerMask.GetMask("Default");
            TargetFinder = new EntityFinder(LookRadius, _enemyMask, _servantMask | lowBarrier | defaultLayerMask);
        }

        public abstract void StartFirstState();
        
        public override void OnDead()
        {
            if (Point is not null)
                KingController.ReturnPoint(Point);
            KingController.RemoveServant(this);
            base.OnDead();
        }
    }
}