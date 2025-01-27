using System.Collections;
using Enemy.FSM;
using Enemy.GoblinWarrior;
using UnityEngine;
using VContainer;

namespace Enemy.GoblinArcher
{
    public class GoblinArcherStayState : EnemyState<GoblinArcherController>
    {
        private readonly IObjectResolver _container;
        private IEnumerator _checkEnvCoroutine;

        public GoblinArcherStayState(IObjectResolver container)
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
                    Enemy.Fsm.ChangeState<GoblinArcherAttackState>();          
            } 
        }

        public override void Dispose()
        {
            Enemy.StopCoroutine(_checkEnvCoroutine);
        }
    }
}