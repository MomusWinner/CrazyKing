using Controllers.UpgradeController;

namespace Servant.Upgrade
{
    public abstract class ServantUpgrade<T> : IUpgradable<T>
    {
        public abstract ServantUpgradeData GetUpgradeData();
       
        public abstract void Upgrade(T entity);

        public abstract void Downgrade(T entity);
    }

    public class ServantUpgradeData
    {
        public int Price { get; set; }
        public string Description { get; set; }
    }
}