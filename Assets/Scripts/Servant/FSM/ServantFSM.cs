using System;
using System.Collections;
using BaseEntity;
using BaseEntity.States;
using FSM;
using King;
using UnityEngine;
using VContainer;

namespace Servant.FSM
{
    public class ServantFSM: FsmController
    {
        public bool AI = true;
        private KingController _king;
        private ServantController _servant;
        private readonly IObjectResolver _container;
        private readonly EntityStateFactory _stateFactory;
        private readonly float _minDistanceToKing = 10f;
        private AttackCondition _attackCondition;
        private EntityState _attackState;
        private EntityState _followToKingState;
        private IEnumerator _checkAttackCondition;
        
        public ServantFSM(IObjectResolver container, EntityStateFactory stateFactory, KingController king)
        {
            _container = container;
            _stateFactory = stateFactory;
            _king = king;
        }

        public void Setup(ServantController servant, EntityStateType attackState, EntityStateType followToKingState)
        {
            _attackCondition = new AttackCondition(servant);
            _servant = servant;
            _checkAttackCondition = CheckAttack();
            servant.StartCoroutine(_checkAttackCondition);
            
            _followToKingState = _stateFactory.GetState(followToKingState);
            _followToKingState.Setup(servant);
            _attackState = _stateFactory.GetState(attackState);
            _attackState.Setup(servant);
            ChangeState(_followToKingState);
        }
        
        public override void ChangeState<T>()
        {
            ChangeState(CreateState<T>());
        }
        
        public IEnumerator CheckAttack()
        {
            while (true)
            {
                if (!AI)
                {
                    yield return null;
                    continue;
                }

                float distanceToKing;
                if (_king == null)
                    distanceToKing = float.MaxValue;
                else
                    distanceToKing = (_king.transform.position - _servant.transform.position).sqrMagnitude;
                
                bool correctDistanceToKing = distanceToKing <= _minDistanceToKing * _minDistanceToKing;
                
                if (currentState != _attackState && correctDistanceToKing)
                {
                    if (_attackCondition.Check())
                    {
                        ChangeState(_attackState);
                        _attackState.OnComplete += () => ChangeState(_followToKingState);
                    }
                }

                if (currentState != _followToKingState && !correctDistanceToKing)
                {
                    ChangeState(_followToKingState);
                    yield return new WaitForSeconds(1f);
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

        public T CreateState<T>()
        {
            throw new NotImplementedException();
            // var state = _container.Resolve<T>();
            // if (state is not ServantState servantState) return default;
            // servantState.Setup(_servant);
            // return state;
        }
    }
    
    public class AttackCondition
    {
        private readonly EntityController _entity;

        public AttackCondition(EntityController entity)
        {
            _entity = entity;
        }

        public bool Check()
        {
            Transform target = _entity.FindTargetInLookRadius();
            if (target != null)
                return true;
            return false;
        }
    }
}