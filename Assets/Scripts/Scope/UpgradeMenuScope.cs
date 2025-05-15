using UI.Upgrade;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scope
{
    public class UpgradeMenuScope : LifetimeScope
    {
        [SerializeField] private EvolutionPanel _evolutionPanel;
        [SerializeField] private UpgradePanel _upgradePanel;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_evolutionPanel).As<EvolutionPanel>();
            builder.RegisterInstance(_upgradePanel).As<UpgradePanel>();
        }
    }
}