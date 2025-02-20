using Controllers;
using Cysharp.Threading.Tasks;
using PlayablesStudio.Plugins.YandexGamesSDK.Runtime;
using UnityEngine;
using VContainer;

public class Bootstrap : MonoBehaviour
{
    [Inject] private SceneLoader _sceneLoader;
    [Inject] private IYandexManager _yandexManager;

    public async UniTask Start()
    {
        DontDestroyOnLoad(gameObject);
        await _sceneLoader.LoadScene("Menu");
        _yandexManager.GameplayReady();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Delete all PlayerPrefs data");
            PlayerPrefs.DeleteAll();
        }
    }
}