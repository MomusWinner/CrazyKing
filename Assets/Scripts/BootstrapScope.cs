using Controllers;
using Controllers.SoundManager;
using King;
using King.Upgrades.Parameters;
using Servant;
using Servant.FSM;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class BootstrapScope : LifetimeScope
{
    [SerializeField] private int _startingCoins;
    [SerializeField] private string _loadingScreenObject = "Loading/Loading"; 
    [SerializeField] private ServantsSO _servantsSO;
    [SerializeField] private ServantStatesSO servantStatesSo;
    [SerializeField] private KingParametersSO _kingParametersSO;
    [SerializeField] private LevelSO _levelSO;
    [SerializeField] private InputActionAsset _actionAsset;
    [SerializeField] private SoundSO _soundSO;

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }       
    
    protected override void Configure(IContainerBuilder builder)
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        builder.Register<MockYandexManager>(Lifetime.Singleton).As<IYandexManager>();       
#else
        builder.Register<YandexManager>(Lifetime.Singleton).As<IYandexManager>();
#endif
        
        builder.RegisterInstance(_soundSO);
        builder.RegisterEntryPoint<SoundManager>().AsSelf();
        
        builder.RegisterInstance(_levelSO);
        builder.RegisterEntryPoint<LevelManager>().AsSelf();
        
        // SaveManager
        SaveManager saveManager = new SaveManager();
        saveManager.Load();
        builder.RegisterInstance(saveManager);
        
        // Input
        builder.RegisterInstance(_actionAsset);
        builder.RegisterEntryPoint<InputManager>().AsSelf();
        
        // Servant
        builder.RegisterInstance(servantStatesSo);
        builder.RegisterEntryPoint<ServantStorage>().AsSelf();
        builder.RegisterInstance(_servantsSO);
        
        // King
        builder.RegisterInstance(_kingParametersSO);
        builder.RegisterEntryPoint<KingParameterManager>().AsSelf();
        
        builder.RegisterEntryPoint<CoinsManager>().WithParameter(typeof(int), _startingCoins).AsSelf();
        builder.RegisterEntryPoint<SceneLoader>().WithParameter(typeof(string), _loadingScreenObject).AsSelf();
    }
}
