using UnityEngine;

namespace Controllers
{
    [CreateAssetMenu(menuName = "Game/LevelsSO")]
    public class LevelsSO : ScriptableObject
    {
        public LevelSO[] Levels;
    }
}