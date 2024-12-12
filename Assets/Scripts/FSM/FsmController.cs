namespace FSM
{
    public abstract class FsmController
    {
        public IState currentState;

        public virtual void ChangeState(IState state)
        {
            currentState?.Dispose();
            currentState = state;
            currentState?.Start();
        }

        public void Update()
        {
            currentState?.Update();
        }

        public void FixedUpdate()
        {
            currentState?.FixedUpdate();
        }

        public void SendMessage(string name, object message)
        {
            currentState?.Message(name, message);
        }
        
        public abstract void ChangeState<T>() where T : IState;
    }
}