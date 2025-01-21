using System;
using Servant.Upgrade;

namespace Servant.Archer.Upgrades
{
    [Serializable]
    public class ArcherUpgradeData : ServantUpgradeData
    {
        public ArcherParamData archerParamData;
    }
}