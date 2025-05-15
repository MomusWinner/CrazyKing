using DG.Tweening;
using Entity.King;
using UnityEngine;
using UnityEngine.UI;

namespace Castle
{
    public class CastleController : MonoBehaviour
    {
        public bool IsCaptured { get; private set; }

        [SerializeField] private SpriteRenderer _roof;
        [SerializeField] private float _captureTime = 3f;
        [SerializeField] private Image _stateImage;
        [SerializeField] private Sprite _targetState;
        [SerializeField] private Sprite _captureStatus;
        [SerializeField] private Sprite _completeState;

        private LayerMask _kingMask;

        private bool _isCapturing;
        private float _elapsedTime;

        public void Start()
        {
            _kingMask = LayerMask.GetMask("King");
            _stateImage.sprite = _targetState;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (1 << other.gameObject.layer != _kingMask)
                return;

            if (other.gameObject.TryGetComponent(out KingController king))
            {
                _roof.DOColor(new Color(1,1,1, 0.3f), 0.5f);
                if (_isCapturing || IsCaptured) return;
                _isCapturing = true;
                _stateImage.sprite = _captureStatus;
            }
        }
        
        public void OnTriggerExit2D(Collider2D collision)
        {
            
            if (1 << collision.gameObject.layer != _kingMask)
                return;

            if (collision.gameObject.TryGetComponent(out KingController _))
            {
                _roof.DOColor(new Color(1,1,1, 1f), 0.5f);
                if (IsCaptured || !_isCapturing) return;
                _elapsedTime = 0;
                _stateImage.sprite = _targetState;
                _stateImage.fillAmount = 1;
                _isCapturing = false;
            }
        }

        public void Update()
        {
            if (!_isCapturing) return;
            
            _elapsedTime += Time.deltaTime;
            _stateImage.fillAmount = _elapsedTime / _captureTime;
            if (_captureTime < _elapsedTime)
            {
                IsCaptured = true;
                _stateImage.sprite = _completeState;
            }
        }
    }
}