using System;
using System.Collections;
using BaseEntity;
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
        private int _attack = Animator.StringToHash("Attack");
        private bool _isAttack;
        private IEnumerator _checkTargetsCoroutine;
        private IWarrior _warrior;
        
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
                _isAttack = true;
                StartAttackAnim();
            }
        }

        public void Dispose()
        {
            _warrior.Controller.Stop();
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
                _warrior.Controller.Stop();
            }
            else
                _warrior.Controller.Move(_target.transform.position);
        }

        private void StartAttackAnim()
        {
            OnStartAttack?.Invoke();
            _soundManager.StartMusic("WarriorAttack", SoundChannel.Effect, Random.Range(0.9f, 1.1f));
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
        public EntityController Controller { get; }
        public int AttackDamage { get; }
        public float AttackRadius { get; }
    }
}