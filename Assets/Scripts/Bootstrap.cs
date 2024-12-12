using Controllers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class Bootstrap : MonoBehaviour
{
    [Inject] private SceneLoader _sceneLoader;

    public async UniTask Start()
    {
        await _sceneLoader.LoadScene("Menu");
    }
}
