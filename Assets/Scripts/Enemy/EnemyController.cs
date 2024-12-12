using BaseEntity;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : EntityController
    {
        private LayerMask _enemyMask;
        private LayerMask _servantMask;

        protected override void Start()
        {
            base.Start();
            _servantMask = LayerMask.GetMask("King");
            _enemyMask = LayerMask.GetMask("Enemy");
        }
        
        public Transform FindKingOrServant()
        {
            return FindObjectInLookRadius(_servantMask, _enemyMask);
        }
    }
}