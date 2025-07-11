﻿using Controllers;
using Cysharp.Threading.Tasks;
using Entity.Servant;
using Servant;
using UnityEngine;
using VContainer;

public class Bootstrap : MonoBehaviour
{
    [Inject] private SceneLoader _sceneLoader;
    [Inject] private IYandexManager _yandexManager;
    [Inject] private SaveManager _saveManager;
    [Inject] private ServantStorage _servantStorage;

    public async UniTask Start()
    {
        DontDestroyOnLoad(gameObject);
        
        if (_saveManager.GameData.IsFirstStarting)
        {
            // TODO: some logic
        }
        await _sceneLoader.LoadScene("Menu");
        
        _yandexManager.GameplayReady();
    }

    public void OnDestroy()
    {
        if (_saveManager.GameData.IsFirstStarting)
        {
            _saveManager.GameData.IsFirstStarting = false;
            _saveManager.Save();
        }
    }

    public void Update()
    {
#if DEBUG
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Delete all PlayerPrefs data");
            PlayerPrefs.DeleteAll();
        }
#endif
    }
}