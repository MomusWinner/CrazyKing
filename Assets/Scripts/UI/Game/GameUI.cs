using System.Collections;
using System.Collections.Generic;
using Controllers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Game
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _castleCounterText;
        [SerializeField] private TMP_Text _enemyCounterText;
        [SerializeField] private GameEndPanel _gameEndPanel;
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _goToUpgradeMenuButton;

        [Inject] private EnemyManager _enemyManager;
        [Inject] private CastleManager _castleManager;
        [Inject] private SceneLoader _sceneLoader;

        public void Update()
        {
            _castleCounterText.text = (_castleManager.CastleCount - _castleManager.CompletedCastlesCount).ToString();
            _enemyCounterText.text = _enemyManager.EnemyCount.ToString();
        }


        public void ShowTitleText(string text, float duration)
        {
            Color startColor = _titleText.color;
            _titleText.text = text;
            Color endColor = startColor; 
            endColor.a = 1;
            var sequence = DOTween.Sequence();
            sequence.Append(_titleText.DOColor(endColor, 0.3f));
            sequence.AppendInterval(duration);
            sequence.Append(_titleText.DOColor(startColor, 0.2f));
        }

        public void OpenLevelEndPanel()
        {
            _gameEndPanel.gameObject.SetActive(true);
        }

        public void SetupGameEndPanel(string title, int coins, int capturedCastles, int killedEnemies)
        {
            _gameEndPanel.Setup(title, coins, capturedCastles, killedEnemies);
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
            _pauseButton.gameObject.SetActive(false);
        }

        public void DisableGoToUpgradeMenuButton()
        {
            _goToUpgradeMenuButton.gameObject.SetActive(false);
        }
    }
}