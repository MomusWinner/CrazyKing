using PlayablesStudio.Plugins.YandexGamesSDK.Runtime;
using UnityEngine;

public class YandexManager : IYandexManager
{
    public void GameplayReady()
    {
        YandexGamesSDK.Instance.SetGameplayReady();
    }

    public void GameplayStart()
    {
        YandexGamesSDK.Instance.SetGameplayStart();
    }

    public void GameplayStop()
    {
        YandexGamesSDK.Instance.SetGameplayStop();
    }
}

public class MockYandexManager : IYandexManager
{
    public void GameplayReady()
    {
        Debug.Log("[Yandex] Gameplay Ready");
    }

    public void GameplayStart()
    {
        Debug.Log("[Yandex] Gameplay Start");
    }

    public void GameplayStop()
    {
        Debug.Log("[Yandex] Gameplay Stop");
    }
}


public interface IYandexManager
{
    public void GameplayReady();
    public void GameplayStart();
    public void GameplayStop();
}