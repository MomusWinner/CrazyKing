using Controllers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class BootstrapScope : LifetimeScope
{
    [SerializeField] private string _loadingScreenObject = "Loading/Loading";
    
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(new SceneLoader(_loadingScreenObject)).AsImplementedInterfaces();
    }
}
