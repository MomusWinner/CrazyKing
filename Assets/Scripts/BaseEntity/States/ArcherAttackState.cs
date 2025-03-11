using System;
using System.Collections;
using Agent;
using Finders;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace BaseEntity.States
{
    public class ArcherAttackState : EntityState
    {
        private readonly int _aimAnim = Animator.StringToHash("isAiming");
        private IArcher _archer;
        private PhysicAgent _agent;
        private BaseEntityController _target;
        private EntityFinder _entityFinder;
        private bool _isAttacking;
        private bool _isAiming;
        private float _attackTimOut;

        private float _attackTimOutLeft;

        private IEnumerator _findTargetCoroutine;
        
        [Inject] private IObjectResolver _container;
        public override void Start()
        {
            _archer = Entity.GetComponent<IArcher>();
            _agent = Entity.GetComponent<PhysicAgent>();
            _attackTimOut = _archer.AttackTimeOut;
            _entityFinder = Entity.TargetFinder;
            _findTargetCoroutine = FindNewTarget();
            Entity.StartCoroutine(_findTargetCoroutine);
        }
        
        public override void Update()
        {
            if (Entity == null) return;

            if (_isAttacking)
            {
                if (_attackTimOutLeft <= 0)
                    Entity.StartCoroutine(ShootDelay());     
                else
                    _attackTimOutLeft -= Time.deltaTime;
            }
        }

        public override void FixedUpdate()
        { 
            if (_target == null) return;
            Entity.Rotate(_target.transform.position - Entity.transform.position, Time.fixedDeltaTime);
            if (_isAiming && IsAimed())
            {
                StartAttack();
            }

            if (!_isAttacking)
            {
                _agent.Stop();
                if (!_isAiming)
                {
                    Entity.Animator.SetBool(_aimAnim, true);
                    _isAiming = true;
                }
            }
        }

        public override void Dispose()
        {
            if (Entity != null)
            {
                _isAiming = false;
                _isAttacking = false;
                OnComplete = null;
                Entity.StopCoroutine(_findTargetCoroutine);
                Entity.Animator.SetBool(_aimAnim, false);
                _findTargetCoroutine = null;
            }
        }

        private void StartAttack()
        { 
            _isAiming = false;
            _isAttacking = true;
            _attackTimOutLeft = _attackTimOut;
        }

        private void Shoot()
        {
            if(_target == null) return;
            GameObject arrowPref = Resources.Load<GameObject>(_archer.ArrowPath);
            Vector2 entityPos = Entity.transform.position;
            Vector2 dir = _target.transform.position - Entity.transform.position;
            dir.Normalize();
            Vector3 spawnPos = entityPos + dir * 0.3f;
            IArrow arrow = _container.Instantiate(arrowPref, spawnPos, Quaternion.Euler(new Vector3(0,0,dir.ToAngle()))).GetComponent<IArrow>();
            arrow.Setup(_archer.ArrowData, dir);
        }

        private IEnumerator ShootDelay()
        {
            _isAttacking = false;
            yield return new WaitForSeconds(Random.Range(0f, 0.5f));
            Shoot();
        }

        private bool IsAimed()
        {
            Vector2 dir = _target.transform.position - Entity.transform.position;
            dir.Normalize();
            if (dir.SqrMagnitude() - MathUtils.AngleToVector2(_target.transform.rotation.z).SqrMagnitude() < 0.001f)
                return true;
            return false;
        }

        private IEnumerator FindNewTarget()
        {
            while (true)
            {
                _target = Entity.FindTargetInLookRadius()?.GetComponent<BaseEntityController>();
                if (_target == null)
                    OnComplete?.Invoke();

                yield return new WaitForSeconds(1f);
            }
        }
    }

    public interface IArcher
    {
        public float AttackTimeOut { get; set; }
        public ArrowData ArrowData { get; }
        public string ArrowPath { get; set; }
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