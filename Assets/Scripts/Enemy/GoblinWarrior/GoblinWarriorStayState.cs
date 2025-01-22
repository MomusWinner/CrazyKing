using System.Collections;
using Enemy.FSM;
using UnityEngine;
using VContainer;

namespace Enemy.GoblinWarrior
{
    public class GoblinWarriorStayState : EnemyState<GoblinWarriorController>
    {
        private readonly IObjectResolver _container;
        private IEnumerator _checkEnvCoroutine;

        public GoblinWarriorStayState(IObjectResolver container)
        {
            _container = container;
        }
        
        public override void Start()
        {
            _checkEnvCoroutine = CheckEnv();
            Enemy.StartCoroutine(_checkEnvCoroutine);
        }

        public IEnumerator CheckEnv()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                Transform target = Enemy.TargetFinder.FindObjectInLookRadius(Enemy.transform.position);
                if (target != null)
                    Enemy.Fsm.ChangeState<GoblinWarriorAttackState>();          
            } 
        }

        public override void Dispose()
        {
            Enemy.StopCoroutine(_checkEnvCoroutine);
        }
    }
}