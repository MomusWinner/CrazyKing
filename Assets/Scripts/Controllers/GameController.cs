using King;
using UI;
using VContainer;
using VContainer.Unity;

namespace Controllers
{
    public class GameController : IStartable
    {
        private int _earnedCoins;
        [Inject] private EnemyManager _enemyManager;
        [Inject] private CastleManager _castleManager;
        [Inject] private CoinsManager _coinsManager;
        [Inject] private GameUI _gameUI;
        [Inject] private KingController _kingController;

        private bool _successComplete;

        public void Start()
        {
            _castleManager.OnCastlesCaptured += SuccessLevelComplete;
            _kingController.OnDeath += FailureLevelComplete;
            _coinsManager.OnIncrease += coins => _earnedCoins += coins;
        }

        public void SuccessLevelComplete()
        {
            _gameUI.OpenLevelEndPanel();
            _gameUI.SetupGameEndPanel("Победа!!", _earnedCoins);
            _successComplete = true;
        }

        public void FailureLevelComplete()
        {
            if(_successComplete) return;
            _gameUI.OpenLevelEndPanel();
            _gameUI.SetupGameEndPanel("Поражение :(", _earnedCoins);
        }
    }
}