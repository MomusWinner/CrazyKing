using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Upgrade.ServantTab
{
    public class ServantParameterUI : MonoBehaviour
    {
        public string FieldName { get; private set; }
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _icon;

        public void Setup(string fieldName, string friendlyName, string text, Sprite icon)
        {
            FieldName = fieldName;
            UpdateText(text);
            _icon.sprite = icon;
        }

        public void UpdateText(string text)
        {
            _text.text = text;
        }
    }
}