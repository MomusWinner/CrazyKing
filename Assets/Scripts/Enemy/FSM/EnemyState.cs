using FSM;

namespace Enemy.FSM
{
    public abstract class EnemyState<TEnemy> : IState where TEnemy:EnemyController
    {
        public TEnemy Enemy { get; private set; }
        
        public virtual void Setup(TEnemy enemy)
        {
            Enemy = enemy;
        }
        
        public virtual void Start()
        { }

        public virtual void Update()
        { }

        public virtual void Message(string name, object obj)
        { }

        public virtual void FixedUpdate()
        { }
        
        public virtual void Dispose()
        { }
    }
}