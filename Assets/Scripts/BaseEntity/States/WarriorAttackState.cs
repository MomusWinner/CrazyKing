using System;
using System.Collections;
using Agent;
using Controllers.SoundManager;
using EntityBehaviour;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace BaseEntity.States
{
    public class WarriorAttackState : EntityState
    {
        [Inject] private IObjectResolver _container;
        private BaseEntityController _target;
        private readonly int _attack = Animator.StringToHash("Attack");
        private bool _isAttack;
        private IEnumerator _checkTargetsCoroutine;
        
        [Inject] private SoundManager _soundManager;
        private IWarrior _warrior;
        private PhysicAgent _physicAgent;

        public override void Start()
        {
            _physicAgent = Entity.GetComponent<PhysicAgent>();
            _warrior = Entity.GetComponent<IWarrior>();
            _warrior.OnAttack += Attack;
            _checkTargetsCoroutine = CheckTargets();
            _warrior.Controller.StartCoroutine(_checkTargetsCoroutine);
        }

        public override void Update()
        {
            if (!_target)
            {
                if(!TryFindEnemyInLookRadius())
                    return;
            }

            float distanceToTarget = Vector2.Distance(_target.transform.position, _warrior.Controller.transform.position);
            if (distanceToTarget <= _warrior.AttackRadius)
            {
                if (!_isAttack)
                {
                    _isAttack = true;
                    StartAttackAnim();
                }
            }
        }

        public override void Dispose()
        {
            OnComplete = null;
            _isAttack = false;
            _target = null;
            _physicAgent.Stop();
            if (_warrior.Controller != null)
                _warrior.Controller.StopCoroutine(_checkTargetsCoroutine);
            _checkTargetsCoroutine = null;
        }
        
        private IEnumerator CheckTargets()
        {
            while (true)
            {
                TryFindEnemyInLookRadius();
                yield return new WaitForSeconds(0.7f);
            }
        }

        private bool TryFindEnemyInLookRadius()
        {
            _target = _warrior.Controller.TargetFinder.FindObjectInLookRadius(_warrior.Controller.transform.position)
                ?.GetComponent<BaseEntityController>();
            if (!_target) OnComplete?.Invoke();
            return _target;
        }
        
        public override void FixedUpdate()
        {
            if(!_target) return;
            if (_isAttack)
            {
                Vector2 dir = _target.transform.position - _warrior.Controller.transform.position;
                _warrior.Controller.Rotate(dir.normalized, Time.fixedDeltaTime);
                _physicAgent.Stop();
            }
            else
                _physicAgent.Move(_target.transform.position);
        }

        private void StartAttackAnim()
        {
            _soundManager.StartMusic("WarriorAttack", SoundChannel.Effect);
            _warrior.Controller.Animator.SetTrigger(_attack);
        }

        public void Attack()
        {
            _isAttack = false;
            if (_target is null) return;
            if (_target.GetDistanceBetweenEntities(_warrior.Controller) > _warrior.AttackRadius)
                return;
            if (_target.Damage(_warrior.AttackDamage))
            {
                _target = null;
                TryFindEnemyInLookRadius();
            }
        }
    }
    
    public interface IWarrior
    {
        public Action OnAttack { get; set; }
        public EntityController Controller { get; }
        public int AttackDamage { get; }
        public float AttackRadius { get; }
    }
}