using Controllers.Coins;
using Cysharp.Threading.Tasks;
using Entity.King;
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
        [Inject] private LevelManager _levelManager;
        [Inject] private SaveManager _saveManager;

        private bool _successComplete;

        public void Start()
        {
            _startCastleAmount = _castleManager.CastleCount;
            _enemyManager.OnEnemyDies += () => _killedEnemies++;
            _castleManager.OnCastlesCaptured += () => _ = SuccessLevelComplete();
            _kingController.OnDeath += () => _ = FailureLevelComplete();
            _coinsManager.OnIncrease += (_, coins)  => _earnedCoins += coins;

            if (_levelManager.Level == 1)
                _gameUI.DisableGoToUpgradeMenuButton();

            LevelSO levelSO = _levelManager.GetCurrentLevelData();
            if (levelSO.CustomStartText)
                _gameUI.ShowTitleText(levelSO.StartText, 5);
            else
                _gameUI.ShowTitleText($"До босса осталось {_levelManager.LevelsToBoss()} уровней", 5);
        }

        public async UniTask SuccessLevelComplete()
        {
            _capturedCastles = _startCastleAmount - (_startCastleAmount - _castleManager.CastleCount);
            
            _gameUI.DisablePausePanel();
            _saveManager.Save();
            await UniTask.Delay(1000);
            _gameUI.OpenLevelEndPanel();
            _gameUI.SetupGameEndPanel("Победа!!", _earnedCoins, _capturedCastles, _killedEnemies);
            _levelManager.CompleteLevel();
            _successComplete = true;
        }

        public async UniTask FailureLevelComplete()
        {
            if(_successComplete) return;
            
            _capturedCastles = _startCastleAmount - (_startCastleAmount - _castleManager.CastleCount);
            
            _gameUI.DisablePausePanel();
            _saveManager.Save();
            await UniTask.Delay(1000);
            _gameUI.OpenLevelEndPanel();
            _gameUI.SetupGameEndPanel("Поражение :(", _earnedCoins, _capturedCastles, _killedEnemies);
        }
    }
}