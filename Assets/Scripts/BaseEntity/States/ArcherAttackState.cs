using System;
using System.Collections;
using Agent;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace BaseEntity.States
{
    public class ArcherAttackState : EntityState
    {
        private readonly int _aimAnim = Animator.StringToHash("isAiming");
        private IArcher _archer;
        private PhysicAgent _agent;
        private BaseEntityController _target;
        private bool _isAttacking;
        private float _attackTimOut;
        private const float StartDelay = 0.5f;

        private float _attackTimOutLeft;
        private bool _isFirstAttack = true;

        private IEnumerator _findTargetCoroutine;
        
        [Inject] private IObjectResolver _container;
        public override void Start()
        {
            _archer = Entity.GetComponent<IArcher>();
            _agent = Entity.GetComponent<PhysicAgent>();
            _attackTimOut = _archer.AttackTimeOut;
            _findTargetCoroutine = FindNewTarget();
            Entity.StartCoroutine(_findTargetCoroutine);
            Entity.Animator.SetBool(_aimAnim, true);
        }
        
        public override void Update()
        {
            if (Entity == null) return;

            if (_isAttacking || _target == null) return;
            Entity.StartCoroutine(StartAttack());
        }

        public override void FixedUpdate()
        {
            if (_target != null)
                RotateToTarget();
        }


        public override void Dispose()
        {
            if (Entity == null) return;
            _isAttacking = false;
            OnComplete = null;
            Entity.StopCoroutine(_findTargetCoroutine);
            Entity.Animator.SetBool(_aimAnim, false);
            _findTargetCoroutine = null;
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

        private IEnumerator StartAttack()
        {
            _isAttacking = true;
            _agent.Stop();

            float aimingTime = 0;
            while (_target != null && !IsAimed())
            {
                aimingTime += Time.deltaTime;
                yield return null;
            }
            
            if (_isFirstAttack && aimingTime > 0  && aimingTime < StartDelay)
                yield return new WaitForSeconds(StartDelay - aimingTime);
            _isFirstAttack = false;
            
            
            if (_target != null)
                Shoot();
                
            yield return new WaitForSeconds(_attackTimOut);
            _isAttacking = false;
        }

        private bool IsAimed()
        {
            Vector2 dir = _target.transform.position - Entity.transform.position;
            dir.Normalize();
            Vector2 entityDir = Entity.transform.right;
            if ((entityDir - dir).sqrMagnitude < 0.01f)
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

                yield return new WaitForSeconds(0.5f);
            }
        }

        private void RotateToTarget()
        {
            Entity.Rotate(_target.transform.position - Entity.transform.position, Time.fixedDeltaTime);
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