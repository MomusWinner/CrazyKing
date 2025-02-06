using System;
using System.Collections;
using BaseEntity;
using BaseEntity.States;
using Finders;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace EntityBehaviour
{
    public class ArcherAttackBehaviour : IDisposable
    {
        public bool Enable { get; set; } = true;
        public Action OnAim;
        public Action OnAttack;
        public Action OnAttackCompleted;

        private EntityController _entity;
        private BaseEntityController _target;
        private readonly string _arrowPath;
        private readonly EntityFinder _entityFinder;
        private readonly ArrowData _arrowData;
        private bool _isAttacking;
        private bool _isAiming;
        private float _attackTimOut;

        private float _attackTimOutLeft;

        private IEnumerator _findTargetCoroutine;
        
        [Inject] private IObjectResolver _container;
        
        public ArcherAttackBehaviour(
            EntityController entityController, 
            string arrowPath,
            float attackTimOut,
            ArrowData arrowData)
        {
            _entity = entityController;    
            _arrowPath = arrowPath;
            _attackTimOut = attackTimOut;
            _entityFinder = _entity.TargetFinder;
            _arrowData = arrowData;
        }

        public void Start()
        {
            if (!Enable) return;
            _findTargetCoroutine = FindNewTarget();
            _entity.StartCoroutine(_findTargetCoroutine);
        }

        public void Update(float dt)
        {
            if (!Enable || _entity == null) return;

            if (_isAttacking)
            {
                if (_attackTimOutLeft <= 0)
                    _entity.StartCoroutine(ShootDelay());     
                else
                    _attackTimOutLeft -= dt;
            }
        }

        public void FixedUpdate(float dt)
        { 
            if (!Enable || _target == null) return;
            _entity.Rotate(_target.transform.position - _entity.transform.position, dt);
            if (_isAiming && IsAimed())
            {
                StartAttack();
            }

            if (!_isAttacking)
            {
                // _entity.Stop(); TODO 
                if (!_isAiming)
                {
                    OnAim?.Invoke();
                    _isAiming = true;
                }
            }
        }

        public void Dispose()
        {
            if (_entity != null)
            {
                _entity.StopCoroutine(_findTargetCoroutine);
            }
        }
        

        private void StartAttack()
        {
            _isAiming = false;
            _isAttacking = true;
            OnAttack?.Invoke();
            _attackTimOutLeft = _attackTimOut;
        }

        private void Shoot()
        {
            if(_target == null) return;
            GameObject arrowPref = Resources.Load<GameObject>(_arrowPath);
            Vector2 entityPos = _entity.transform.position;
            Vector2 dir = _target.transform.position - _entity.transform.position;
            dir.Normalize();
            Vector3 spawnPos = entityPos + dir * 0.3f;
            IArrow arrow = _container.Instantiate(arrowPref, spawnPos, Quaternion.Euler(new Vector3(0,0,dir.ToAngle()))).GetComponent<IArrow>();
            arrow.Setup(_arrowData, dir);
        }

        private IEnumerator ShootDelay()
        {
            _isAttacking = false;
            yield return new WaitForSeconds(Random.Range(0f, 0.5f));
            Shoot();
        }

        private bool IsAimed()
        {
            Vector2 dir = _target.transform.position - _entity.transform.position;
            dir.Normalize();
            if (dir.SqrMagnitude() - MathUtils.AngleToVector2(_target.transform.rotation.z).SqrMagnitude() < 0.001f)
                return true;
            return false;
        }

        private IEnumerator FindNewTarget()
        {
            while (true)
            {
                _target = _entityFinder.FindObjectInLookRadius(_entity.transform.position)
                    ?.GetComponent<BaseEntityController>();
                if (_target == null)
                    OnAttackCompleted?.Invoke();

                yield return new WaitForSeconds(1f);
            }
        }
    }
}