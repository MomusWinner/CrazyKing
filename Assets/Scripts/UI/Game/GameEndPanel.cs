using TMPro;
using UnityEngine;

namespace UI.Game
{
    public class GameEndPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private TMP_Text _capturedCastlesText;
        [SerializeField] private TMP_Text _killedEnemiesText;

        public void Setup(string title, int coin, int capturedCastles, int killedEnemies)
        {
            _titleText.text = title;
            _coinsText.text = coin.ToString();
            _capturedCastlesText.text = capturedCastles.ToString();
            _killedEnemiesText.text = killedEnemies.ToString();
        }
    }
}