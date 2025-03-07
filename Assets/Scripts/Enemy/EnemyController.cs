using System.Collections.Generic;
using Agent;
using BaseEntity;
using BaseEntity.States;
using Controllers.Coins;
using Enemy.FSM;
using Finders;
using King;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyController : EntityController, IThrowable
    {
        [Inject] public EnemyFSM FSM;

        [SerializeField] private EntityStateType _attackState;
        [SerializeField] private EntityStateType _defaultState;
        [SerializeField] private int _priceMin = 10;
        [SerializeField] private int _priceMax = 20;
        
        [Inject] private CoinsManager _coinsManager;

        protected override void Start()
        {
            Initialize();
            base.Start();
            TargetFinder = new EntityFinder(
                LookRadius,  
                targetMask: LayerMasks.King,
                ignoreMask: LayerMasks.Enemy| LayerMasks.LowBarrier| LayerMasks.Default);
            
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

        public override void OnDead()
        {
            base.OnDead();
            DropCoins();
        }
        
        public void Throw(Vector2 direction, float force)
        {
            RigidBody.AddForce(direction * force, ForceMode2D.Impulse);
        }

        private void DropCoins()
        {
            float dropRadius = 1.4f;
            int price = Random.Range(_priceMin, _priceMax);
            List<Coin> coins = _coinsManager.GetCoins(price);
            for (int i = 0; i < coins.Count; i++)
            {
                Vector3 randCirclePos = Random.insideUnitCircle;
                coins[i].transform.position = transform.position;
                coins[i].SetStartPosition(coins[i].transform.position + randCirclePos * dropRadius);
            }
        }
    }
}