namespace Controllers.UpgradeController
{
    public abstract class UpgradeController<TEntity>
    {
        protected TEntity entity;
        protected IUpgradable<TEntity>[] upgradables;
        protected int currentUpdate = -1;
        
        protected UpgradeController(TEntity entity)
        {
            this.entity = entity;
        }

        public void Setup()
        {
            upgradables = SetupUpgrades();
        }

        public bool Upgrade()
        {
            if (upgradables.Length - 1 == currentUpdate) return false;
            currentUpdate++;
            upgradables[currentUpdate].Upgrade(entity);
            return true;
        }

        public bool Downgrade()
        {
            if (currentUpdate < 0) return false; 
            upgradables[currentUpdate].Downgrade(entity);
            currentUpdate--;
            return true;
        }

        protected abstract IUpgradable<TEntity>[] SetupUpgrades();
    }

    public interface IUpgradable<T>
    {
        void Upgrade(T entity);

        void Downgrade(T entity);
    }
}