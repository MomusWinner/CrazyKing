using TriInspector;
using UnityEngine;

namespace Controllers
{
    [CreateAssetMenu(menuName = "Game/LevelsSO")]
    public class LevelsSO : ScriptableObject
    {
        public bool IsTestingMode;
        [ShowIf(nameof(IsTestingMode))]
        public int TestLevel;
        public LevelSO[] Levels;
    }
}