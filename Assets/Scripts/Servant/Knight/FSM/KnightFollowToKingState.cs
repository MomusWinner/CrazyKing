using System.Collections;
using King;
using Servant.FSM;
using UnityEngine;

namespace Servant.Knight.FSM
{
    public class KnightFollowToKingState : FollowKingState<KnightController>
    {
        protected override int isRun => Animator.StringToHash("isRun");

        private IEnumerator _checkEnemyCoroutine;
        
        public KnightFollowToKingState(KingController king) : base(king)
        {
        }

        public override void Start()
        {
            base.Start();
            _checkEnemyCoroutine = CheckEnemy();
            Servant.StartCoroutine(_checkEnemyCoroutine);
        }

        public override void Dispose()
        {
            base.Dispose();
            Servant.StopCoroutine(_checkEnemyCoroutine);
        }

        private IEnumerator CheckEnemy()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                Transform enemy = Servant.FindEnemyInLookRadius();
                if (enemy is not null)
                    Servant.Fsm.ChangeState<KnightAttackState>(); // TODO send target to state
            }
        }
    }
}