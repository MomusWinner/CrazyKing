using System.Collections;
using BaseEntity;
using Enemy.FSM;
using Health;
using UnityEngine;

namespace Enemy.RedKnight
{
    public class RedKnightAttackState : EnemyState<RedKnightController>
    {
        private IDamageable _damageable;
        private BaseEntityController _targetObject;
        
        private int _attack = Animator.StringToHash("Attack");
        private bool _isAttack;
        private IEnumerator _checkTargetsCoroutine;
        
        public override void Start()
        {
            _checkTargetsCoroutine = CheckTargets();
            Enemy.StartCoroutine(_checkTargetsCoroutine);
        }

        private IEnumerator CheckTargets()
        {
            while (true)
            {
                TryFindKingOrServant();
                yield return new WaitForSeconds(0.2f);
            }
        }

        public override void Update()
        {
            if (!_targetObject)
            {
                Enemy.Fsm.ChangeState<RedKnightStayState>();
                return;
            }

            float distanceToTarget = Vector2.Distance(_targetObject.transform.position, Enemy.transform.position);
            if (distanceToTarget <= Enemy.AttackRadius)
            {
                _isAttack = true;
                StartAttackAnim();
            }
        }

        public override void FixedUpdate()
        {
            if(!_targetObject) return;
            if (_isAttack)
            {
                Vector2 dir = _targetObject.transform.position - Enemy.transform.position;
                Enemy.Rotate(dir.normalized, Time.fixedDeltaTime);
                Enemy.Stop();
            }
            else
                Enemy.SetDestination(_targetObject.transform.position);
        }
        
        public override void Message(string name, object obj)
        {
            switch (name)
            {
                case "attack":
                    Attack();
                    break;
            }
        }

        public override void Dispose()
        {
            Enemy.Stop();
            Enemy.StopCoroutine(_checkTargetsCoroutine);
        }

        public void Attack()
        {
            _isAttack = false;
            if (!_targetObject) return;
            if (_targetObject.GetDistanceBetweenEntities(Enemy) > Enemy.AttackRadius)
                return;
            if (_damageable.Damage(Enemy.AttackDamage))
            {
                _damageable = null;
                
                if(!TryFindKingOrServant())
                    Enemy.Fsm.ChangeState<RedKnightStayState>();
            }
        }
        
        private void StartAttackAnim()
        {
            Enemy.Animator.SetTrigger(_attack);
        }
        
        private bool TryFindKingOrServant()
        {
            _targetObject = Enemy.FindKingOrServant()?.gameObject.GetComponent<BaseEntityController>();
            _damageable = _targetObject?.GetComponent<IDamageable>();
            return _damageable is not null;
        }
    }
}