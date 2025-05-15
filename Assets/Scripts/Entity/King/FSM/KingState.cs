using FSM;

namespace Entity.King.FSM
{
    public abstract class KingState : IState
    {
        public KingController King { get; private set; }

        public virtual void Setup(KingController king)
        {
            King = king;
        }
        
        public virtual void Start()
        { }

        public virtual void Update()
        { }

        public virtual void Message(string name, object obj = null)
        { }

        public virtual void FixedUpdate()
        { }

        public virtual void Dispose()
        { }
    }
}