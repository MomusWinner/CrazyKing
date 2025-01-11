using UnityEngine;

namespace Servant
{
    [CreateAssetMenu(menuName = "Game/Servant/" + nameof(ServantSO))]
    public class ServantSO: ScriptableObject
    {
        public string servantName;
        public string description;
        public Sprite avatar;
        public ServantType type;
        public string PrefabPath;
    }
}