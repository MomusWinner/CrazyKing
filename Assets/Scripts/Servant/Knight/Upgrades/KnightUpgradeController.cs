using Controllers.UpgradeController;

namespace Servant.Knight.Upgrades
{
    public class KnightUpgradeController : UpgradeController<KnightController>
    {
        public KnightUpgradeController(KnightController entity) : base(entity)
        {
        }

        protected override IUpgradable<KnightController>[] SetupUpgrades()
        {
            return new IUpgradable<KnightController>[]
            {
                new Knight1Upgrade(),
                new Knight2Upgrade(),
                new Knight3Upgrade(),
                new Knight4Upgrade(),
                new Knight5Upgrade()
            };
        }
    }
}