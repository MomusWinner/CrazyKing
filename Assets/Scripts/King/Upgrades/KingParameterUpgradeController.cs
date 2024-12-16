using Controllers.UpgradeController;
using JetBrains.Annotations;

namespace King.Upgrades
{
    public abstract class KingParameterUpgradeController
    {
        public abstract string Title { get; }
        
        public int NextUpgradePrice => NextUpgrade.Price;
        
        public string NextUpgradeDescription => NextUpgrade?.Description;

        public int Length => (int)upgradeController?.Length;
        
        [CanBeNull] public KingUpgrade CurrentUpgrade => upgradeController.CurrentUpgrade as KingUpgrade;
        
        [CanBeNull] public KingUpgrade NextUpgrade => upgradeController.NextUpgrade as KingUpgrade;
        
        public int currentIndex => upgradeController.CurrentUpdateIndex;

        protected readonly KingUpgrade[] kingUpgrades;
        protected readonly UpgradeController<KingController> upgradeController;
        
        public KingParameterUpgradeController(KingController entity, KingUpgrade[] kingUpgrades)
        {
            this.kingUpgrades = kingUpgrades;
            upgradeController = new UpgradeController<KingController>(entity, this.kingUpgrades);
        }

        public bool Upgrade()
        {
            // TODO check money
            return upgradeController.Upgrade();
        }
    }
}