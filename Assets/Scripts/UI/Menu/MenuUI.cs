using Controllers;
using UnityEngine;
using VContainer;

public class MenuUI : MonoBehaviour
{
    [Inject] private SceneLoader _sceneLoader;
    [Inject] private SaveManager _saveManager;

    public void StartGame()
    {
        if (_saveManager.GameData.IsFirstStarting)
            _sceneLoader?.LoadScene("Tutorial");
        else
            _sceneLoader?.LoadScene("UpgradeMenu");
    }

    public void OpenOptions()
    {
        Debug.Log("Options is not implemented");
    }
}
