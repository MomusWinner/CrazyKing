using Controllers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class Bootstrap : MonoBehaviour
{
    [Inject] private SceneLoader _sceneLoader;

    public async UniTask Start()
    {
        DontDestroyOnLoad(gameObject);
        await _sceneLoader.LoadScene("Menu");
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