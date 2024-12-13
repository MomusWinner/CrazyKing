using Controllers.UpgradeController;

namespace Servant.Knight.Upgrades
{
    public class Knight1Upgrade : IUpgradable<KnightController>
    {
        public void Upgrade(KnightController entity)
        {
            entity.transform.localScale *= 2;
        }

        public void Downgrade(KnightController entity)
        {
            entity.transform.localScale /= 2;
        }
    }
    
    public class Knight2Upgrade : IUpgradable<KnightController>
    {
        public void Upgrade(KnightController entity)
        {
            entity.transform.localScale *= 2;
        }

        public void Downgrade(KnightController entity)
        {
            entity.transform.localScale /= 2;
        }
    }
    
    public class Knight3Upgrade : IUpgradable<KnightController>
    {
        public void Upgrade(KnightController entity)
        {
            entity.transform.localScale *= 2;
        }

        public void Downgrade(KnightController entity)
        {
            entity.transform.localScale /= 2;
        }
    }
    
    public class Knight4Upgrade : IUpgradable<KnightController>
    {
        public void Upgrade(KnightController entity)
        {
            entity.transform.localScale *= 2;
        }

        public void Downgrade(KnightController entity)
        {
            entity.transform.localScale /= 2;
        }
    }
    
    public class Knight5Upgrade : IUpgradable<KnightController>
    {
        public void Upgrade(KnightController entity)
        {
            entity.transform.localScale *= 2;
        }

        public void Downgrade(KnightController entity)
        {
            entity.transform.localScale /= 2;
        }
    }
}