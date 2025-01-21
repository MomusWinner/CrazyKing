using System.Collections;
using King;
using Servant.FSM;
using UnityEngine;

namespace Servant.Archer.FSM
{
    public class ArcherFollowToKingState : FollowKingState<ArcherController>
    {
        protected override int isRun => Animator.StringToHash("isRuning");
        private IEnumerator _checkEnemyCoroutine;
        
        public ArcherFollowToKingState(KingController king, ServantStatesSO statesSO) : base(king, statesSO)
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
                Transform enemy = Servant.FindTargetInLookRadius();
                if (enemy is not null)
                    Servant.Fsm.ChangeState<ArcherAttackState>();
            }
        }
    }
}