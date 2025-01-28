using King;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Castle
{
    public class CastleController : MonoBehaviour
    {
        public bool IsCaptured { get; private set; }

        [FormerlySerializedAs("_capturingTime")] [SerializeField] private float _captureTime;
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
            if (1 << other.gameObject.layer != _kingMask || _isCapturing || IsCaptured)
                return;

            if (other.gameObject.TryGetComponent(out KingController king))
            {
                _isCapturing = true;
                _stateImage.sprite = _captureStatus;
            }
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            
            if (1 << collision.gameObject.layer != _kingMask || IsCaptured)
                return;

            if (collision.gameObject.TryGetComponent(out KingController king) && _isCapturing)
            {
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