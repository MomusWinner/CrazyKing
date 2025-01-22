using Enemy.FSM;
using Enemy.GoblinWarrior;
using Enemy.RedKnight;
using King;
using King.FSM;
using Servant;
using Servant.Archer;
using Servant.Archer.FSM;
using Servant.FSM;
using Servant.Knight;
using Servant.Knight.FSM;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scope
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private KingController _kingController;
        
        protected override void Configure(IContainerBuilder builder)
        {
            // Register King
            builder.RegisterComponent(_kingController);
            builder.Register<KingAttackState>(Lifetime.Scoped);
            builder.Register<DefaultKingState>(Lifetime.Scoped);
            
            // SERVANTS
            builder.RegisterEntryPoint<ServantManager>().AsSelf();
            builder.Register<ServantFactory>(Lifetime.Singleton);
            
            // Register Knight
            builder.Register<KnightFollowToKingState>(Lifetime.Transient);
            builder.Register<KnightAttackState>(Lifetime.Transient);
            builder.Register<ServantFSM<KnightController>>(Lifetime.Transient);
            
            // Register Archer
            builder.Register<ArcherFollowToKingState>(Lifetime.Transient);
            builder.Register<ArcherAttackState>(Lifetime.Transient);
            builder.Register<ServantFSM<ArcherController>>(Lifetime.Transient);
            
            // ENEMIES
            
            // Register Goblin Warrior
            builder.Register<EnemyFSM<GoblinWarriorController>>(Lifetime.Transient);
            builder.Register<GoblinWarriorAttackState>(Lifetime.Transient);
            builder.Register<GoblinWarriorStayState>(Lifetime.Transient);
            
            // Register RedKnight
            // builder.Register<EnemyFSM<RedKnightController>>(Lifetime.Transient);
            // builder.Register<RedKnightStayState>(Lifetime.Transient);
            // builder.Register<RedKnightAttackState>(Lifetime.Transient);
        }
    }
}
