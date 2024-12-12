using System;

namespace FSM
{
    public interface IState : IDisposable
    {
        void Start();

        void Update();

        void Message(string name, object obj);

        void FixedUpdate();
    }
}