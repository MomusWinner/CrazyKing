using BaseEntity;
using BaseEntity.States;
using Enemy.FSM;
using Finders;
using UnityEngine;
using VContainer;

namespace Enemy
{
    public class EnemyController : EntityController
    {
        [Inject] public EnemyFSM FSM;

        [SerializeField] private EntityStateType _attackState;
        [SerializeField] private EntityStateType _defaultState;
        
        private LayerMask _enemyMask;
        private LayerMask _servantMask;

        protected override void Start()
        {
            Initialize();
            base.Start();
            _servantMask = LayerMask.GetMask("King");
            _enemyMask = LayerMask.GetMask("Enemy");
            LayerMask lowBarrier = LayerMask.GetMask("LowBarrier");
            LayerMask defaultLayer = LayerMask.GetMask("Default");
            TargetFinder = new EntityFinder(LookRadius, _servantMask, _enemyMask | lowBarrier | defaultLayer);
            
            
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
    }
}