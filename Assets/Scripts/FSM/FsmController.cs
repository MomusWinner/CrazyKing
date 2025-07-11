﻿namespace FSM
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

        public virtual void Update()
        {
            currentState?.Update();
        }

        public virtual void FixedUpdate()
        {
            currentState?.FixedUpdate();
        }

        public void SendMessage(string name, object message = null)
        {
            currentState?.Message(name, message);
        }
        
        public abstract void ChangeState<T>() where T : IState;
    }
}