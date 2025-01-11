using Servant.Upgrade;

namespace Servant.Knight.Upgrades
{
    public class Knight1Upgrade : ServantUpgrade<KnightController>
    {
        public override ServantUpgradeData GetUpgradeData()
        {
            return new ServantUpgradeData()
            {
                Description = "Knight 1 upgrade",
                Price = 10,
            };
        }

        public override void  Upgrade(KnightController entity)
        {
            entity.transform.localScale *= 2;
        }

        public override void Downgrade(KnightController entity)
        {
            entity.transform.localScale /= 2;
        }
    }
    
    public class Knight2Upgrade : ServantUpgrade<KnightController>
    {
        public override ServantUpgradeData GetUpgradeData()
        {
            return new ServantUpgradeData()
            {
                Description = "Knight 2 upgrade",
                Price = 10,
            };
        }

        public override void Upgrade(KnightController entity)
        {
            entity.transform.localScale *= 2;
        }

        public override void Downgrade(KnightController entity)
        {
            entity.transform.localScale /= 2;
        }
    }
    
    public class Knight3Upgrade : ServantUpgrade<KnightController>
    {
        public override ServantUpgradeData GetUpgradeData()
        {
            return new ServantUpgradeData()
            {
                Description = "Knight 3 upgrade",
                Price = 10,
            };
        }

        public override void Upgrade(KnightController entity)
        {
            entity.transform.localScale *= 2;
        }

        public override void Downgrade(KnightController entity)
        {
            entity.transform.localScale /= 2;
        }
    }
    
    public class Knight4Upgrade : ServantUpgrade<KnightController>
    {
        public override ServantUpgradeData GetUpgradeData()
        { 
            return new ServantUpgradeData()
            {
                Description = "Knight 4 upgrade",
                Price = 10,
            };
        }

        public override void Upgrade(KnightController entity)
        {
            entity.transform.localScale *= 2;
        }

        public override void Downgrade(KnightController entity)
        {
            entity.transform.localScale /= 2;
        }
    }
    
    public class Knight5Upgrade : ServantUpgrade<KnightController>
    {
        public override ServantUpgradeData GetUpgradeData()
        {
            return new ServantUpgradeData()
            {
                Description = "Knight 5 upgrade",
                Price = 10,
            };
        }

        public override void Upgrade(KnightController entity)
        {
            entity.transform.localScale *= 2;
        }

        public override void Downgrade(KnightController entity)
        {
            entity.transform.localScale /= 2;
        }
    }
}