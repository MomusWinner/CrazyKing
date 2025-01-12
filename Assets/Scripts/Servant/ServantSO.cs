using System.Collections.Generic;
using Servant.Upgrade;
using UnityEngine;
using UnityEngine.Serialization;

namespace Servant
{
    [CreateAssetMenu(menuName = "Game/Servant/" + nameof(ServantSO))]
    public class ServantSO: ScriptableObject
    {
        public string servantName;
        public string description;
        public Sprite avatar;
        public ServantType type;
        public string prefabPath;
        public List<ServantUpgradeSO> upgrades;
        public int price;
    }

    public enum ServantType
    {
        Knight,
    }
}