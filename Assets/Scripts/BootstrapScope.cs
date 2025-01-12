using Controllers;
using King;
using King.Upgrades.Parameters;
using Servant;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class BootstrapScope : LifetimeScope
{
    [SerializeField] private int _startingCoins;
    [SerializeField] private string _loadingScreenObject = "Loading/Loading"; 
    [SerializeField] private ServantsSO _servantsSO;
    [SerializeField] private KingParametersSO _kingParametersSO;

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }       
    
    protected override void Configure(IContainerBuilder builder)
    {
        // Servant
        builder.RegisterEntryPoint<ServantsStorage>().AsSelf();
        builder.RegisterInstance(_servantsSO);
        // King
        builder.RegisterInstance(_kingParametersSO);
        builder.RegisterEntryPoint<KingParameterManager>().AsSelf();
        
        builder.RegisterEntryPoint<CoinsManager>().WithParameter(typeof(int), _startingCoins).AsSelf();
        builder.RegisterEntryPoint<SceneLoader>().WithParameter(typeof(string), _loadingScreenObject).AsSelf();
    }
}
