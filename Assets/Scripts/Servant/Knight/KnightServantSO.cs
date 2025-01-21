using System.Collections.Generic;
using System.Linq;
using Servant.Knight.Upgrades;
using Servant.Upgrade;
using UnityEngine;

namespace Servant.Knight
{
    [CreateAssetMenu(menuName = "Game/Servant/" + nameof(KnightServantSO))]
    public class KnightServantSO : ServantSO
    {
        public KnightParamData startParam;
        public List<KnightUpgradeData> upgrades; 
        public override List<ServantUpgradeData> Upgrades => upgrades.Cast<ServantUpgradeData>().ToList();
    }
}