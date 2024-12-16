using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace Controllers
{
    public class SceneLoader: IStartable
    {
        private readonly string _loadingScreenPath;
        private GameObject _loadingScreen;
        
        public SceneLoader(string loadingScreenPath)
        {
            _loadingScreenPath = loadingScreenPath;
            Debug.Log("Initialize SceneLoader");
        }
        
        public void Start()
        {
            var loadingScreen = Resources.Load<GameObject>(_loadingScreenPath);
            if (loadingScreen is null)
            {
                Debug.LogError("Incorrect loading screen object path" );
            }
            _loadingScreen = Object.Instantiate(loadingScreen);
            Object.DontDestroyOnLoad(_loadingScreen);
            _loadingScreen.SetActive(false);
        }
        
        public async UniTask LoadScene(string name)
        {
            _loadingScreen.SetActive(true);
            await SceneManager.LoadSceneAsync(name);
            _loadingScreen.SetActive(false);
        }
    }
}