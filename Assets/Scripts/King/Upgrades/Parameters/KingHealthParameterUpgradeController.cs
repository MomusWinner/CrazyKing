namespace King.Upgrades.Parameters
{
    public class KingHealthParameterUpgradeController : KingParameterUpgradeController
    {
        public override string Title => "Здоровье";

        public KingHealthParameterUpgradeController(KingController entity, KingUpgrade[] kingUpgrades) : base(entity, kingUpgrades)
        {
        }
    }
    
    public class KingHealthParameter1Upgrade : KingUpgrade
    {
        public override int Price => 7;
        public override string Description => "Увеличить здоровье на 50 ед.";
        
        public  override void Upgrade(KingController entity) => entity.ChangeMaxHealth(entity.MaxHealth + 40);

        public override void Downgrade(KingController entity) => entity.ChangeMaxHealth(entity.MaxHealth - 40);
    }
}