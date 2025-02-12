using System.Collections;
using BaseEntity.States;
using FSM;
using King;
using UnityEngine;
using VContainer;

namespace Enemy.FSM
{
    public class EnemyFSM : FsmController
    {
        public bool AI = true;
        private EnemyController _enemy;
        private readonly IObjectResolver _container;
        private readonly EntityStateFactory _stateFactory;
        private readonly KingController _king;
        private EntityState _attackState;
        private EntityState _defaultState;
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

            _attackState = _stateFactory.GetState(attackState);
            _attackState.Setup(enemy);
            
            _defaultState = _stateFactory.GetState(defaultState);
            _defaultState.Setup(enemy);
            
            ChangeState(_defaultState);
            
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
                if (currentState != _attackState)
                {
                    Transform target = _enemy.TargetFinder.FindObjectInLookRadius(_enemy.transform.position);
                    if (target != null)
                    { 
                        ChangeState(_attackState); 
                        _attackState.OnComplete += () => ChangeState(_defaultState);
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