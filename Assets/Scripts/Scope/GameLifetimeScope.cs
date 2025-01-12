using Enemy.FSM;
using Enemy.RedKnight;
using King;
using King.FSM;
using Servant;
using Servant.FSM;
using Servant.Knight;
using Servant.Knight.FSM;
using UI.Upgrade.ServantTab;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace Scope
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private KingController _kingController;
        [FormerlySerializedAs("_servantTab")] [SerializeField] private ServantContainer servantContainer;
        
        protected override void Configure(IContainerBuilder builder)
        {
            // Register King
            builder.RegisterComponent(_kingController);
            builder.Register<KingFSM>(Lifetime.Scoped);
            builder.Register<ServantFactory>(Lifetime.Singleton);
            
            // Register Knight
            builder.Register<KnightFollowToKingState>(Lifetime.Transient);
            builder.Register<KnightAttackState>(Lifetime.Transient);
            builder.Register<ServantFSM<KnightController>>(Lifetime.Transient);
            
            // Register RedKnight
            builder.Register<EnemyFSM<RedKnightController>>(Lifetime.Transient);
            builder.Register<RedKnightStayState>(Lifetime.Transient);
            builder.Register<RedKnightAttackState>(Lifetime.Transient);
        }
    }
}
