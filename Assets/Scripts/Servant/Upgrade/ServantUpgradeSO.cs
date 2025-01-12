using UnityEngine;

namespace Servant.Upgrade
{
    [CreateAssetMenu(menuName = "Game/Servant/Upgrades/" + nameof(ServantUpgradeSO))]
    public class ServantUpgradeSO : ScriptableObject
    {
        public string description;
        public int price;
    }
}