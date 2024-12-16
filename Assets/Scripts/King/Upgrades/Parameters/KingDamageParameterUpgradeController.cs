using Controllers;

namespace King.Upgrades.Parameters
{
    public class KingDamageParameterUpgradeController : KingParameterUpgradeController
    {
        public override string Title => "Урон";

        public KingDamageParameterUpgradeController(
            KingController entity,
            KingUpgrade[] kingUpgrades,
            CoinsController coinsController) : base(entity, kingUpgrades, coinsController)
        {
        }
    }
    
    public class KingDamageParameter1Upgrade : KingUpgrade
    {
        public override int Price => 5;
        public override string Description => "Увеличить урон на 5 ед.";

        public override void Upgrade(KingController entity)
        { }

        public override void Downgrade(KingController entity)
        { }
    }
    
    public class KingDamageParameter2Upgrade : KingUpgrade
    {
        public override int Price => 10;
        public override string Description => "Увеличить урон на 10 ед.";

        public override void Upgrade(KingController entity)
        { }

        public override void Downgrade(KingController entity)
        { }
    }
    
    public class KingDamageParameter3Upgrade : KingUpgrade
    {
        public override int Price => 15;
        public override string Description => "Увеличить урон на 15 ед.";

        public override void Upgrade(KingController entity)
        { }

        public override void Downgrade(KingController entity)
        { }
    }
    
    public class KingDamageParameter4Upgrade : KingUpgrade
    {
        public override int Price => 25;
        public override string Description => "Увеличить урон на 25 ед.";

        public override void Upgrade(KingController entity)
        { }

        public override void Downgrade(KingController entity)
        { }
    }
    
    public class KingDamageParameter5Upgrade : KingUpgrade
    {
        public override int Price => 60;
        public override string Description => "Увеличить урон на 60 ед.";

        public override void Upgrade(KingController entity)
        { }

        public override void Downgrade(KingController entity)
        { }
    }
}