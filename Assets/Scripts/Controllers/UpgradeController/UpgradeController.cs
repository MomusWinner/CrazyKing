using JetBrains.Annotations;

namespace Controllers.UpgradeController
{
    public class UpgradeController<TEntity>: IUpgradeController
    {
        public int Level => _currentUpdateIndex + 1;

        public int MaxLevel => Length; 
        
        [CanBeNull]
        public IUpgradable<TEntity> CurrentUpgrade
        {
            get
            {
                if (_currentUpdateIndex < 0 || _currentUpdateIndex > upgradables.Length  - 1)
                    return null;
                return upgradables[_currentUpdateIndex];
            }
        }

        [CanBeNull]
        public IUpgradable<TEntity> NextUpgrade
        {
            get
            {
                int nextUpgrade = _currentUpdateIndex + 1;
                if (nextUpgrade < 0 || nextUpgrade > upgradables.Length  - 1)
                    return null;
                return upgradables[nextUpgrade];
            }
        }
        
        public int CurrentUpdateIndex => _currentUpdateIndex;
        
        public int Length => (int)upgradables?.Length;

        protected TEntity entity;
        protected IUpgradable<TEntity>[] upgradables;
        
        private int _currentUpdateIndex = -1;

        public UpgradeController(TEntity entity, IUpgradable<TEntity>[] upgradables)
        {
            this.entity = entity;
            this.upgradables = upgradables;
        }

        public virtual bool Upgrade()
        {
            if (upgradables.Length - 1 == _currentUpdateIndex) return false;
            _currentUpdateIndex++;
            upgradables[_currentUpdateIndex].Upgrade(entity);
            return true;
        }

        public virtual bool Downgrade()
        {
            if (_currentUpdateIndex < 0) return false; 
            upgradables[_currentUpdateIndex].Downgrade(entity);
            _currentUpdateIndex--;
            return true;
        }
    }

    public interface IUpgradeController
    {
        int Level { get; }
        
        int MaxLevel { get; }

        bool Upgrade();

        bool Downgrade();
    }
    
    public interface IUpgradable<T>
    {
        void Upgrade(T entity);

        void Downgrade(T entity);
    }
}