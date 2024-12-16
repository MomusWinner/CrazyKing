using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProgressBarByCells: MonoBehaviour
    {
        public int MaxValue => _maxValue;
        
        [SerializeField] private Sprite _emptyCell;
        [SerializeField] private Sprite _currentCell;
        [SerializeField] private Sprite _filledCell;
        [SerializeField] private GameObject _cellsParent;
        [SerializeField] private GameObject _cellPref;

        private int _maxValue;
        private Image[] _cells;

        public void SetUp(int maxValue)
        {
            _cells = new Image[maxValue];
            if (maxValue > 0)
                _maxValue = maxValue;

            for (int i = 0; i < _maxValue; i++)
            {
                 var cell = Instantiate(_cellPref, _cellsParent.transform).GetComponent<Image>();
                 _cells[i] = cell;
            }
        }

        public void SetCurrentValue(int value)
        {
            if (value > _maxValue)
                value = _maxValue;
            else if (value < 1)
                value = 1;

            for (int i = 0; i < _maxValue; i++)
            {
                if (i < value)
                    _cells[i].sprite = i == value - 1 ? _currentCell : _filledCell;
                else
                    _cells[i].sprite = _emptyCell;
            }
        }

        public void SetFullValue()
        {
            for (int i = 0; i < _maxValue; i++)
            {
                _cells[i].sprite = _filledCell;
            }
        }
    }
}