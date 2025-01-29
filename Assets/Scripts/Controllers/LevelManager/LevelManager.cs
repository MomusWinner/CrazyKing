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
        [Inject] private LevelSO _levelSO;
        
        public void Start()
        {
            Level = _saveManager.GameData.Level;
        }

        public async UniTask LoadLevel()
        {
            await _sceneLoader.LoadScene(_levelSO.levels[Level - 1]);
        }

        public void CompleteLevel()
        {
            if (Level >= _levelSO.levels.Length)
                return;
            Level++;
            _saveManager.GameData.Level = Level;
            _saveManager.Save();
        }
    }
}