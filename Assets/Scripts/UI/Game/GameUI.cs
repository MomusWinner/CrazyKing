using Controllers;
using TMPro;
using UnityEngine;
using VContainer;

namespace UI.Game
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _castleCounterText;
        [SerializeField] private TMP_Text _enemyCounterText;
        [SerializeField] private GameEndPanel _gameEndPanel;
        [SerializeField] private GameObject _pausePanel;

        [Inject] private EnemyManager _enemyManager;
        [Inject] private CastleManager _castleManager;
        [Inject] private SceneLoader _sceneLoader;

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

        public void MoveToUpgradeScene()
        {
            _sceneLoader.LoadScene("UpgradeMenu");
        }

        public void OpenPausePanel()
        {
            Time.timeScale = 0;
            _pausePanel.SetActive(true);
        }

        public void ClosePausePanel()
        {
            Time.timeScale = 1;
            _pausePanel.SetActive(false);
        }

        public void DisablePausePanel()
        {
            
        }
    }
}