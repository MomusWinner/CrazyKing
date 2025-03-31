using System;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Controllers
{
    public class LevelManager : IStartable 
    {
        public int Level { get; private set; }
        [Inject] private SaveManager _saveManager;
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private LevelsSO _levelsSO;
        
        public void Start()
        {
            Level = _saveManager.GameData.Level;
        }

        public async UniTask LoadLevel()
        {
            await _sceneLoader.LoadScene(_levelsSO.Levels[Level - 1].SceneName);
        }

        public LevelSO GetCurrentLevelData()
        {
            return _levelsSO.Levels[Level - 1];
        }

        public int LevelsToBoss()
        {
            int bossLevelIndex = Array.FindIndex(_levelsSO.Levels, l => l.BossLevel);
            if (bossLevelIndex == -1) return -1;
            return bossLevelIndex - Level + 1;
        }

        public void CompleteLevel()
        {
            if (Level >= _levelsSO.Levels.Length)
                return;
            Level++;
            _saveManager.GameData.Level = Level;
            _saveManager.Save();
        }
    }
}