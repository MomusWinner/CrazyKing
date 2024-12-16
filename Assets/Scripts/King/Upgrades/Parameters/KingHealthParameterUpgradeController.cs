using Controllers;

namespace King.Upgrades.Parameters
{
    public class KingHealthParameterUpgradeController : KingParameterUpgradeController
    {
        public override string Title => "Здоровье";

        public KingHealthParameterUpgradeController(
            KingController entity,
            KingUpgrade[] kingUpgrades,
            CoinsController coinsController) : base(entity, kingUpgrades, coinsController)
        {
        }
    }
    
    public class KingHealthParameter1Upgrade : KingUpgrade
    {
        public override int Price => 7;
        const int HealthProfit = 10;
        public override string Description => $"Увеличить здоровье на {HealthProfit} ед.";

        
        public  override void Upgrade(KingController entity) => entity.ChangeMaxHealth(entity.MaxHealth + HealthProfit);

        public override void Downgrade(KingController entity) => entity.ChangeMaxHealth(entity.MaxHealth - HealthProfit);
    }
    
    public class KingHealthParameter2Upgrade : KingUpgrade
    {
        public override int Price => 20;
        const int HealthProfit = 25;
        public override string Description => $"Увеличить здоровье на {HealthProfit} ед.";

        
        public  override void Upgrade(KingController entity) => entity.ChangeMaxHealth(entity.MaxHealth + HealthProfit);

        public override void Downgrade(KingController entity) => entity.ChangeMaxHealth(entity.MaxHealth - HealthProfit);
    }
    
    public class KingHealthParameter3Upgrade : KingUpgrade
    {
        public override int Price => 40;
        const int HealthProfit = 25;
        public override string Description => $"Увеличить здоровье на {HealthProfit} ед.";

        
        public  override void Upgrade(KingController entity) => entity.ChangeMaxHealth(entity.MaxHealth + HealthProfit);

        public override void Downgrade(KingController entity) => entity.ChangeMaxHealth(entity.MaxHealth - HealthProfit);
    }
    
    public class KingHealthParameter4Upgrade : KingUpgrade
    {
        public override int Price => 60;
        const int HealthProfit = 30;
        public override string Description => $"Увеличить здоровье на {HealthProfit} ед.";

        
        public  override void Upgrade(KingController entity) => entity.ChangeMaxHealth(entity.MaxHealth + HealthProfit);

        public override void Downgrade(KingController entity) => entity.ChangeMaxHealth(entity.MaxHealth - HealthProfit);
    }
    
    public class KingHealthParameter5Upgrade : KingUpgrade
    {
        public override int Price => 100;
        const int HealthProfit = 35;
        public override string Description => $"Увеличить здоровье на {HealthProfit} ед.";

        
        public  override void Upgrade(KingController entity) => entity.ChangeMaxHealth(entity.MaxHealth + HealthProfit);

        public override void Downgrade(KingController entity) => entity.ChangeMaxHealth(entity.MaxHealth - HealthProfit);
    }
}