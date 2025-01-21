using System.Collections.Generic;
using System.Linq;
using Servant.Archer.Upgrades;
using Servant.Upgrade;
using UnityEngine;

namespace Servant.Archer
{
    [CreateAssetMenu(menuName = "Game/Servant/" + nameof(ArcherServantSO))]
    public class ArcherServantSO : ServantSO
    {
        public ArcherParamData startParam;
        public List<ArcherUpgradeData> upgrades;
        public override List<ServantUpgradeData> Upgrades => upgrades.Cast<ServantUpgradeData>().ToList();
    }
}