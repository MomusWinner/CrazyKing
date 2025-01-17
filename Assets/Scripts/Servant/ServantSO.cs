using System.Collections.Generic;
using Servant.Upgrade;
using UnityEngine;

namespace Servant
{
    [CreateAssetMenu(menuName = "Game/Servant/" + nameof(ServantSO))]
    public abstract class ServantSO: ScriptableObject
    {
        public string servantName;
        public string description;
        public Sprite avatar;
        public ServantType type;
        public string prefabPath;
        public int price;
        public abstract List<ServantUpgradeData> Upgrades { get; }
    }

    public enum ServantType
    {
        Knight,
    }
}