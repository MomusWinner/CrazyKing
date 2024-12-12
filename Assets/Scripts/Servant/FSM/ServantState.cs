
using FSM;

namespace Servant.FSM
{
    public abstract class ServantState<TServant> : IState where TServant:ServantController
    {
        public TServant Servant => _servant;
        
        private TServant _servant;
        
        public void SetUp(TServant servant)
        {
            _servant = servant;
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