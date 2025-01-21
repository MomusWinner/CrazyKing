using System.Collections;
using Enemy;
using Servant.FSM;
using UnityEngine;

namespace Servant.Knight.FSM
{
    public class KnightAttackState : ServantState<KnightController>
    {
        private EnemyController _target;
        private int _attack = Animator.StringToHash("Attack");
        private bool _isAttack;
        private IEnumerator _checkTargetsCoroutine;
        
        public override void Start()
        {
            _checkTargetsCoroutine = CheckTargets();
            Servant.StartCoroutine(_checkTargetsCoroutine);
        }

        public override void Update()
        {
            if (!_target)
            {
                Servant.Fsm.ChangeState<KnightFollowToKingState>();
                return;
            }

            float distanceToTarget = Vector2.Distance(_target.transform.position, Servant.transform.position);
            if (distanceToTarget <= Servant.AttackRadius)
            {
                _isAttack = true;
                StartAttackAnim();
            }
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
            Servant.Stop();
            Servant.StopCoroutine(_checkTargetsCoroutine);
        }
        
        private IEnumerator CheckTargets()
        {
            while (true)
            {
                TryFindEnemyInLookRadius();
                yield return new WaitForSeconds(0.2f);
            }
        }

        private bool TryFindEnemyInLookRadius()
        {
            _target = Servant.TargetFinder.FindObjectInLookRadius(Servant.transform.position)
                ?.GetComponent<EnemyController>();
            return _target;
        }
        
        public override void FixedUpdate()
        {
            if(!_target) return;
            if (_isAttack)
            {
                Vector2 dir = _target.transform.position - Servant.transform.position;
                Servant.Rotate(dir.normalized, Time.fixedDeltaTime);
                Servant.Stop();
            }
            else
                Servant.SetDestination(_target.transform.position);
        }

        private void StartAttackAnim()
        {
            Servant.Animator.SetTrigger(_attack);
        }

        public void Attack()
        {
            _isAttack = false;
            if (_target is null) return;
            if (_target.GetDistanceBetweenEntities(Servant) > Servant.AttackRadius)
                return;
            if (_target.Damage(Servant.AttackDamage))
            {
                _target = null;
                
                if(!TryFindEnemyInLookRadius())
                    Servant.Fsm.ChangeState<KnightFollowToKingState>();
            }
        }
    }
}