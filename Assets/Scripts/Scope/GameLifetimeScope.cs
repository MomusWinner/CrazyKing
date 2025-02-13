using BaseEntity.States;
using Controllers;
using Enemy.FSM;
using King;
using King.FSM;
using Servant;
using Servant.FSM;
using UI.Game;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scope
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private KingController _kingController;
        [SerializeField] private GameUI _gameUI;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<LevelBootstrap>();
            
            builder.RegisterInstance(_gameUI);
            
            builder.RegisterEntryPoint<CastleManager>().AsSelf();
            builder.RegisterEntryPoint<GameController>().AsSelf();
            
            // Register King
            builder.RegisterComponent(_kingController);
            builder.Register<KickState>(Lifetime.Transient);
            builder.Register<KingAttackState>(Lifetime.Transient);
            builder.Register<DefaultKingState>(Lifetime.Transient);
            
            // ENTITY STATES
            EntityStateFactory.RegisterStates(builder);
            builder.Register<EntityStateFactory>(Lifetime.Singleton);
            
            // SERVANTS
            builder.Register<ServantFSM>(Lifetime.Transient);
            builder.RegisterEntryPoint<ServantManager>().AsSelf();
            builder.Register<ServantFactory>(Lifetime.Singleton);
            
            // ENEMIES
            builder.RegisterEntryPoint<EnemyManager>().AsSelf();
            builder.Register<EnemyFSM>(Lifetime.Transient);
        }
    }
}
