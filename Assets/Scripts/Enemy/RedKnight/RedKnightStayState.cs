using System.Collections;
using System.Linq;
using Enemy.FSM;
using UnityEngine;
using VContainer;

namespace Enemy.RedKnight
{
    public class RedKnightStayState : EnemyState<RedKnightController>
    {
        private readonly IObjectResolver _container;
        private IEnumerator _checkEnvCoroutine;

        public RedKnightStayState(IObjectResolver container)
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
                Collider2D[] colliders =  Physics2D.OverlapCircleAll(
                    Enemy.transform.position,
                    Enemy.LookRadius,
                    LayerMask.GetMask("King"));

                if (!colliders.Any()) continue;

                Enemy.Fsm.ChangeState<RedKnightAttackState>();          
            } 
        }

        public override void Dispose()
        {
            Enemy.StopCoroutine(_checkEnvCoroutine);
        }
    }
}