using Controllers.SoundManager;
using VContainer;
using VContainer.Unity;

public class LevelBootstrap : IStartable
{
    [Inject] private SoundManager _soundManager;
        
    public void Start()
    {
        _soundManager.StartMusic("Forest", SoundChannel.Background);
    }
}