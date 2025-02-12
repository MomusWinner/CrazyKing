using System.Collections;
using Agent;
using BaseEntity;
using BaseEntity.States;
using Enemy.FSM;
using Finders;
using King;
using UnityEngine;
using VContainer;

namespace Enemy
{
    public class EnemyController : EntityController, IThrowable
    {
        [Inject] public EnemyFSM FSM;

        [SerializeField] private EntityStateType _attackState;
        [SerializeField] private EntityStateType _defaultState;
        
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

        public void Throw(Vector2 direction, float force)
        {
            _agent.Stop();
            _agent.FreezeRotation = true;
            _agent.FreezeMovement = true;
            FSM.AI = false;
            RigidBody.AddForce(direction * force, ForceMode2D.Impulse);
            StartCoroutine(Wakeup());
        }

        private IEnumerator Wakeup()
        {
            yield return new WaitForSeconds(1f);
            FSM.AI = true;
            _agent.FreezeMovement = false;
            _agent.FreezeRotation = false;
        }
    }
}