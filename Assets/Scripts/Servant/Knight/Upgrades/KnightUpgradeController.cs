using Controllers.UpgradeController;
using Servant.Upgrade;

namespace Servant.Knight.Upgrades
{
    public class KnightUpgradeController : ServantUpgradeController<KnightController>
    {
        public KnightUpgradeController(KnightController entity, IUpgradable<KnightController>[] knightUpgrades)
            : base(entity, knightUpgrades)
        { }
    }
}