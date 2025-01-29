using TMPro;
using UnityEngine;

namespace UI.Game
{
    public class GameEndPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _coinsText;

        public void Setup(string title, int coin)
        {
            _titleText.text = title;
            _coinsText.text = $"+ {coin}";
        }
    }
}