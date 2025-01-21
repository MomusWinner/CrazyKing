using System;
using BaseEntity;
using Finders;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EntityBehaviour
{
    public class ArcherAttackBehaviour
    {
        public bool Enable { get; set; } = true;
        public Action OnAim;
        public Action OnAttack;
        public Action OnAttackCompleted;

        private EntityController _entity;
        private EntityController _target;
        private readonly string _arrowPath;
        private readonly EntityFinder _entityFinder;
        private readonly ArrowData _arrowData;
        private bool _isAttacking;
        private bool _isAiming;
        private float _attackTimOut;

        private float _attackTimOutLeft;
        
        
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
            FindNewTarget();
        }

        public void Update(float dt)
        {
            if (!Enable) return;

            if (_isAttacking)
            {
                if (_attackTimOutLeft <= 0)
                    Shoot();     
                else
                    _attackTimOutLeft -= dt;
            }

            if (_target == null)
                FindNewTarget();
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
                _entity.Stop(); 
                if (!_isAiming)
                {
                    OnAim?.Invoke();
                    _isAiming = true;
                }
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
            IArrow arrow = Object.Instantiate(arrowPref, spawnPos, Quaternion.Euler(new Vector3(0,0,dir.ToAngle()))).GetComponent<IArrow>();
            arrow.Setup(_arrowData, dir);
            _isAttacking = false;
        }

        private bool IsAimed()
        {
            Vector2 dir = _target.transform.position - _entity.transform.position;
            dir.Normalize();
            if (dir.SqrMagnitude() - MathUtils.AngleToVector2(_target.transform.rotation.z).SqrMagnitude() < 0.001f)
                return true;
            return false;
        }

        private void FindNewTarget()
        {
             _target = _entityFinder.FindObjectInLookRadius(_entity.transform.position)?.GetComponent<EntityController>();
             if (_target == null)
                 OnAttackCompleted?.Invoke();
        }
    }

    public interface IArrow
    {
        void Setup(ArrowData data, Vector2 diraction);
    }

    [Serializable]
    public class ArrowData
    {
        public float speed;
        public int damage;
        public float distance;
        public LayerMask targetLayer;
    }
}