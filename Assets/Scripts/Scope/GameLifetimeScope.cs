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
            builder.Register<KingAttackState>(Lifetime.Scoped);
            builder.Register<DefaultKingState>(Lifetime.Scoped);
            
            // SERVANTS
            builder.Register<ServantFSM>(Lifetime.Transient);
            
            //STATES
            EntityStateFactory.RegisterStates(builder);
            builder.Register<EntityStateFactory>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<ServantManager>().AsSelf();
            builder.Register<ServantFactory>(Lifetime.Singleton);
            
            // Register Knight
            
            // Register Archer
            // builder.Register<ArcherAttackState>(Lifetime.Transient);
            // builder.Register<ServantFSM<ArcherController>>(Lifetime.Transient);
            
            // ENEMIES
            
            builder.RegisterEntryPoint<EnemyManager>().AsSelf();
            builder.Register<EnemyFSM>(Lifetime.Singleton);
            
            // Register Goblin Warrior
            // builder.Register<EnemyFSM<GoblinWarriorController>>(Lifetime.Transient);
            // builder.Register<GoblinWarriorAttackState>(Lifetime.Transient);
            // builder.Register<GoblinWarriorStayState>(Lifetime.Transient);
            //
            // // Register Goblin Archer
            // builder.Register<EnemyFSM<GoblinArcherController>>(Lifetime.Transient);
            // builder.Register<GoblinArcherStayState>(Lifetime.Transient);
            // builder.Register<GoblinArcherAttackState>(Lifetime.Transient);
        }
    }
}
