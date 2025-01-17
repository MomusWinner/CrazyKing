using TriInspector;

namespace Servant.Upgrade
{
    public abstract class ServantUpgradeData
    {
        public string description;
        public int price;
        public bool isMergeUpgrade;
        [ShowIf(nameof(isMergeUpgrade))]
        public int mergeAmount;
    }
}