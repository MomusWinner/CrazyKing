using System;
using Servant.Upgrade;

namespace Servant.Knight.Upgrades
{
    [Serializable]
    public class KnightUpgradeData : ServantUpgradeData
    {
        public KnightParamData paramData;
    }
}