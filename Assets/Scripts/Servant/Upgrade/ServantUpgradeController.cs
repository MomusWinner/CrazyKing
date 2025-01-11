using Controllers.UpgradeController;

namespace Servant.Upgrade
{
    public class ServantUpgradeController<T> : UpgradeController<T>, IServantUpgradeController
    {
        public ServantUpgradeController(T entity, IUpgradable<T>[] upgradables) : base(entity, upgradables)
        {
        }

        public ServantUpgradeData GetNextUpgradeData()
        { 
            return ((ServantUpgrade<T>)NextUpgrade).GetUpgradeData();
        }
    }

    public interface IServantUpgradeController : IUpgradeController
    {
        public ServantUpgradeData GetNextUpgradeData();
    }
}