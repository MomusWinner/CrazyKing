using Controllers.Coins;
using King;
using UI.Game;
using VContainer;
using VContainer.Unity;

namespace Controllers
{
    public class GameController : IStartable
    {
        private int _earnedCoins;
        private int _capturedCastles;
        private int _killedEnemies;
        private int _startCastleAmount;
        
        [Inject] private EnemyManager _enemyManager;
        [Inject] private CastleManager _castleManager;
        [Inject] private CoinsManager _coinsManager;
        [Inject] private GameUI _gameUI;
        [Inject] private KingController _kingController;

        private bool _successComplete;

        public void Start()
        {
            _startCastleAmount = _castleManager.CastleCount;
            _enemyManager.OnEnemyDies += () => _killedEnemies++;
            _castleManager.OnCastlesCaptured += SuccessLevelComplete;
            _kingController.OnDeath += FailureLevelComplete;
            _coinsManager.OnIncrease += (_, coins)  => _earnedCoins += coins;
        }

        public void SuccessLevelComplete()
        {
            _capturedCastles = _startCastleAmount - (_startCastleAmount - _castleManager.CastleCount);
            
            _gameUI.DisablePausePanel();
            _gameUI.OpenLevelEndPanel();
            _gameUI.SetupGameEndPanel("Победа!!", _earnedCoins, _capturedCastles, _killedEnemies);
            _successComplete = true;
        }

        public void FailureLevelComplete()
        {
            if(_successComplete) return;
            
            _capturedCastles = _startCastleAmount - (_startCastleAmount - _castleManager.CastleCount);
            
            _gameUI.DisablePausePanel();
            _gameUI.OpenLevelEndPanel();
            _gameUI.SetupGameEndPanel("Поражение :(", _earnedCoins, _capturedCastles, _killedEnemies);
        }
    }
}