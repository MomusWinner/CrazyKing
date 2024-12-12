using System;
using BaseEntity;
using UnityEngine;

namespace Servant
{
    public class ServantController : EntityController
    {
        private LayerMask _enemyMask;
        private LayerMask _servantMask;

        protected override void Start()
        {
            base.Start(); 
            _enemyMask = LayerMask.GetMask("Enemy");
            _servantMask = LayerMask.GetMask("King");
        }
        
        public Transform FindEnemyInLookRadius()
        {
            return FindObjectInLookRadius(_enemyMask, _servantMask);
        }
    }
}