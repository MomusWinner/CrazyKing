using BaseEntity;
using Finders;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : EntityController
    {
        private LayerMask _enemyMask;
        private LayerMask _servantMask;

        protected override void Start()
        {
            Initialize();
            base.Start();
            _servantMask = LayerMask.GetMask("King");
            _enemyMask = LayerMask.GetMask("Enemy");
            LayerMask lowBarrier = LayerMask.GetMask("LowBarrier");
            TargetFinder = new EntityFinder(LookRadius, _servantMask, _enemyMask | lowBarrier);
        }
        
        public Transform FindKingOrServant()
        {
            return FindTargetInLookRadius();
        }
    }
}