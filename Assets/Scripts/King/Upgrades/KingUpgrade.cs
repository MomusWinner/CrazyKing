using Controllers.UpgradeController;

namespace King.Upgrades
{
    public abstract class KingUpgrade: IUpgradable<KingController>
    {
        public abstract int Price { get; }
        public abstract string Description { get; }

        public abstract void Upgrade(KingController entity);

        public abstract void Downgrade(KingController entity);
    }
}