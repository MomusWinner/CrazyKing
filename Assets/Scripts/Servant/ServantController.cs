using BaseEntity;
using Controllers.UpgradeController;
using King;
using Servant.Upgrade;
using UnityEngine;
using VContainer;

namespace Servant
{
    public abstract class ServantController : EntityController
    {
        public abstract IServantUpgradeController UpgradeController { get; }
        
        public ServantSO ServantData { get; set; }
        
        public IPoint Point { get; set; }
        
        [Inject] public KingController KingController;

        private LayerMask _enemyMask;
        private LayerMask _servantMask;
        
        protected override void Start()
        {
            if (Point is null)
                if (KingController.TryGetFreePoint(out IPoint point))
                    Point = point;
            base.Start(); 
            KingController.AddServant(this);
            _enemyMask = LayerMask.GetMask("Enemy");
            _servantMask = LayerMask.GetMask("King");
        }
        
        public Transform FindEnemyInLookRadius()
        {
            return FindObjectInLookRadius(_enemyMask, _servantMask);
        }

        public override void OnDead()
        {
            if (Point is not null)
                KingController.ReturnPoint(Point);
            KingController.RemoveServant(this);
            base.OnDead();
        }
    }
}