using Controllers;
using TMPro;
using UI.Game;
using UnityEngine;
using VContainer;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _castleCounterText;
        [SerializeField] private TMP_Text _enemyCounterText;
        [SerializeField] private GameEndPanel _gameEndPanel;

        [Inject] private EnemyManager _enemyManager;
        [Inject] private CastleManager _castleManager;

        public void Update()
        {
            _castleCounterText.text = (_castleManager.CastleCount - _castleManager.CompletedCastlesCount).ToString();
            _enemyCounterText.text = _enemyManager.EnemyCount.ToString();
        }

        public void OpenLevelEndPanel()
        {
            _gameEndPanel.gameObject.SetActive(true);
        }

        public void SetupGameEndPanel(string title, int coins)
        {
            _gameEndPanel.Setup(title, coins);
        }
    }
    
}