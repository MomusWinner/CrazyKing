using Controllers;
using UnityEngine;
using VContainer;

public class MenuUI : MonoBehaviour
{
    [Inject] private SceneLoader _sceneLoader;

    public void StartGame()
    {
        _sceneLoader?.LoadScene("Level_1");
    }

    public void OpenOptions()
    {
        Debug.Log("Options is not implemented");
    }
}
