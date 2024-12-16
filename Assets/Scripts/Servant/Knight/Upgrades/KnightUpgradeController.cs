using Controllers.UpgradeController;

namespace Servant.Knight.Upgrades
{
    public class KnightUpgradeController : UpgradeController<KnightController>
    {
        public KnightUpgradeController(KnightController entity, IUpgradable<KnightController>[] knightUpgrades)
            : base(entity, knightUpgrades)
        { }
    }
}