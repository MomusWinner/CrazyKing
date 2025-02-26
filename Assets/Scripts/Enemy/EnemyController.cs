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
        private LayerMask _enemyMask;
        private LayerMask _servantMask;
        private PhysicAgent _agent;

        protected override void Start()
        {
            Initialize();
            base.Start();
            _agent = GetComponent<PhysicAgent>();
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