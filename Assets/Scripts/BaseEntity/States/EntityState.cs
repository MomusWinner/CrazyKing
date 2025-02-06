using System;
using FSM;

namespace BaseEntity.States
{
    public abstract class EntityState : IState
    {
        public Action OnComplete { get; set; }
        protected EntityController Entity;

        public void Setup(EntityController entity)
        {
            Entity = entity;
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