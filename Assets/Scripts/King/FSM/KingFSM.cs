using FSM;
using VContainer;

namespace King.FSM
{
    public class KingFSM : FsmController
    {
        private KingController _king;
        private readonly IObjectResolver _container;
        
        public KingFSM(IObjectResolver container, KingController king)
        {
            _container = container;
            _king = king;
        }
        
        public override void ChangeState<T>()
        {
            var state = _container.Resolve<T>();
            if (state is not KingState kingState) return;
            kingState.Setup(_king);
            ChangeState(state);
        }
    }
}