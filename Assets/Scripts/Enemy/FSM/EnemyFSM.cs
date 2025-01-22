using FSM;
using VContainer;

namespace Enemy.FSM
{
    public class EnemyFSM<TEnemy> : FsmController where TEnemy : EnemyController
    {
        private TEnemy _enemy;
        private readonly IObjectResolver _container;
        
        public EnemyFSM(IObjectResolver container)
        {
            _container = container;
        }

        public void Setup(TEnemy enemy)
        {
            _enemy = enemy;
        }
        
        public override void ChangeState<T>()
        {
            var state = _container.Resolve<T>();
            if (state is not EnemyState<TEnemy> enemyState) return;
            enemyState.Setup(_enemy);
            ChangeState(state);
        }
    }
}