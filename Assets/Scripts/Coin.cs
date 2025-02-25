using System.Collections;
using Controllers.CoinsManager;
using DG.Tweening;
using King;
using UnityEngine;
using VContainer;

public class Coin : MonoBehaviour
{
    public int Price { get; set; }
    
    [SerializeField] private float _startSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _defaultScale = 1f;
    [SerializeField] private float _startScale = 0.7f;
    [SerializeField] private float _endScale = 1.1f;
    [SerializeField] private float _moveToStartPositionDuration = .4f;
    
    [Inject] private CoinsManager _coinsManager;
    private LayerMask _kingLayerMask;
    private bool _inStartPosition = true;
    private bool _moveToTarget = false;
    
    public void Start()
    {
        transform.localScale = _defaultScale * Vector3.one;
        _kingLayerMask = LayerMask.GetMask("King");
    }

    public void SetStartPosition(Vector3 startPosition)
    {
        _inStartPosition = false;
        transform.localScale = Vector3.one * _startScale;
        transform
            .DOScale(_endScale, _moveToStartPositionDuration)
            .SetEase(Ease.OutQuart);
        transform
            .DOMove(startPosition, _moveToStartPositionDuration)
            .SetEase(Ease.OutQuart)
            .OnComplete(() =>
            {
                _inStartPosition = true;
                transform.DOScale(_defaultScale, 0.2f);
            });
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (!_inStartPosition || _moveToTarget) return;
        if (1 << other.gameObject.layer != _kingLayerMask) return;
        if (!other.TryGetComponent(out KingController _)) return;
        StartCoroutine(ToTarget(other.transform));
    }

    private IEnumerator ToTarget(Transform target)
    {
        _moveToTarget = true;
        while (true)
        {
            Vector3 direction = target.position - transform.position;
            if (direction.sqrMagnitude < 0.5f)
            {
                _coinsManager.AddCoins(Price);
                DOTween.Kill(gameObject);
                Destroy(gameObject);
                break;
            }
            _startSpeed += _acceleration * Time.deltaTime;
            transform.position += direction.normalized * (_startSpeed * Time.deltaTime); 
            
            yield return null;
        }
    }
}