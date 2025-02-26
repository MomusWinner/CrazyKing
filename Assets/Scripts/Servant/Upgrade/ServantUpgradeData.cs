using TriInspector;
using UnityEngine;

namespace Servant.Upgrade
{
    public abstract class ServantUpgradeData
    {
        [TextArea]
        public string description;
        [HideIf(nameof(isMergeUpgrade))]
        public int price;
        public bool isMergeUpgrade;
        [ShowIf(nameof(isMergeUpgrade))]
        public int mergeAmount;
    }
}