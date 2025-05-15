using System;
using FSM;
using VContainer;

namespace Entity.King.FSM
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
            ChangeState(state);
        }

        public void ChangeState(Type stateType)
        {
            var state = _container.Resolve(stateType);
            ChangeState((IState)state);
        }

        public override void ChangeState(IState state)
        {
            if (state is not KingState kingState || state == null) return;
            kingState.Setup(_king);
            base.ChangeState(state);
        }
    }
}