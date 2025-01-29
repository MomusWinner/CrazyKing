using UnityEngine;

namespace Controllers
{
    [CreateAssetMenu(menuName = "Game/LevelSO")]
    public class LevelSO : ScriptableObject
    {
        public string[] levels;
    }
}