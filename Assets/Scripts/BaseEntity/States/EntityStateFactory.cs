using VContainer;

namespace BaseEntity.States
{
    public enum EntityStateType
    {
        // Walk
        FollowToKing,
        Stay,
        Wander,
        WanderByPoints,
        
        // Attack
        WarriorAttack,
        ArcherAttack,
    }
    
    public class EntityStateFactory
    {
        [Inject] private IObjectResolver _container;

        public static void RegisterStates(IContainerBuilder builder)
        {
            builder.Register<FollowToKingState>(Lifetime.Transient);
            builder.Register<WarriorAttackState>(Lifetime.Transient);
            builder.Register<ArcherAttackState>(Lifetime.Transient);
            builder.Register<WanderState>(Lifetime.Transient);
            builder.Register<WanderByPointState>(Lifetime.Transient);
        }
        
        public EntityState GetState(EntityStateType stateType)
        {
            return stateType switch
            {
                EntityStateType.FollowToKing => _container.Resolve<FollowToKingState>(),
                EntityStateType.WarriorAttack => _container.Resolve<WarriorAttackState>(),
                EntityStateType.ArcherAttack => _container.Resolve<ArcherAttackState>(),
                EntityStateType.Stay => new StayState(),
                EntityStateType.Wander => _container.Resolve<WanderState>(),
                EntityStateType.WanderByPoints => _container.Resolve<WanderByPointState>(),
                _ => null
            };
        }
    }
}