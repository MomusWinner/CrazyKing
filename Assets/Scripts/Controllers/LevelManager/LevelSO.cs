using System;
using TriInspector;

namespace Controllers
{
    [Serializable]
    public class LevelSO
    {
        public string SceneName;

        public bool CustomStartText;
        [ShowIf(nameof(CustomStartText))]
        public string StartText;

        public bool BossLevel;
    }
}