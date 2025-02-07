using System;
using System.Collections;
using BaseEntity;
using BaseEntity.States;
using Controllers.SoundManager;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace EntityBehaviour
{
    public class WarriorAttackBehaviour
    {
        public Action OnComplete;
        public Action OnStartAttack;
        
        private BaseEntityController _target;
        private readonly int _attack = Animator.StringToHash("Attack");
        private readonly IWarrior _warrior;
        private bool _isAttack;
        private IEnumerator _checkTargetsCoroutine;
        
        [Inject] private SoundManager _soundManager;

        public WarriorAttackBehaviour(IWarrior warrior)
        {
            _warrior = warrior;
        }
        
        public void Start()
        {
            _checkTargetsCoroutine = CheckTargets();
            _warrior.Controller.StartCoroutine(_checkTargetsCoroutine);
        }

        public void Update(float dt)
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

        public void Dispose()
        {
            // _warrior.Controller.Stop(); TODO
            _warrior.Controller.StopCoroutine(_checkTargetsCoroutine);
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
        
        public void FixedUpdate(float dt)
        {
            if(!_target) return;
            if (_isAttack)
            {
                Vector2 dir = _target.transform.position - _warrior.Controller.transform.position;
                _warrior.Controller.Rotate(dir.normalized, dt);
                // _warrior.Controller.Stop(); TODO
            }
            // else
                // _warrior.Controller.Move(_target.transform.position); TODO
        }

        private void StartAttackAnim()
        {
            OnStartAttack?.Invoke();
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
}