using System.Collections;
using Entity.King;
using Entity.States;
using FSM;
using UnityEngine;
using VContainer;

namespace Entity.Enemy
{
    public class EnemyFSM : FsmController
    {
        public EntityState AttackState { get; private set; }

        public EntityState DefaultState { get; private set; }

        public bool AI = true;
        private EnemyController _enemy;
        private readonly IObjectResolver _container;
        private readonly EntityStateFactory _stateFactory;
        private readonly KingController _king;
        private IEnumerator _checkStateCoroutine;

        public EnemyFSM(IObjectResolver container, EntityStateFactory stateFactory, KingController king)
        {
            _container = container;
            _stateFactory = stateFactory;
            _king = king;
        }

        public void Setup(EnemyController enemy, EntityStateType attackState, EntityStateType defaultState)
        {
            _enemy = enemy;

            AttackState = _stateFactory.GetState(attackState);
            AttackState.Setup(enemy);
            
            DefaultState = _stateFactory.GetState(defaultState);
            DefaultState.Setup(enemy);
            
            ChangeState(DefaultState);
            
            _checkStateCoroutine = CheckState();
            enemy.StartCoroutine(_checkStateCoroutine);
        }

        public IEnumerator CheckState()
        {
            while (true)
            {
                if (!AI)
                {
                    yield return null;
                    continue;
                }
                if (currentState != AttackState)
                {
                    Transform target = _enemy.FindTargetInLookRadius();
                    if (target != null)
                    { 
                        ChangeState(AttackState); 
                        AttackState.OnComplete += () => ChangeState(DefaultState);
                    }
                }
                 
                yield return new WaitForSeconds(1f);
            }
        }
        
        public override void ChangeState<T>()
        {
            throw new System.NotImplementedException();
        }
    }
}