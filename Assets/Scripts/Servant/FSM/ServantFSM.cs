using FSM;
using VContainer;

namespace Servant.FSM
{
    public class ServantFSM<TServant>: FsmController where TServant:ServantController
    {
        private TServant _servant;
        private readonly IObjectResolver _container;
        
        public ServantFSM(IObjectResolver container)
        {
            _container = container;
        }

        public void SetUp(TServant servant)
        {
            _servant = servant;
        }
        
        public override void ChangeState<T>()
        {
            ChangeState(CreateState<T>());
        }

        public T CreateState<T>()
        {
            var state = _container.Resolve<T>();
            if (state is not ServantState<TServant> servantState) return default;
            servantState.SetUp(_servant);
            return state;
        }
    }
}