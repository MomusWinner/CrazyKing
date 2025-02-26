using BaseEntity.States;
using Controllers.SoundManager;
using Health;
using UnityEngine;
using VContainer;

namespace Arrow
{
    public class BaseArrow : MonoBehaviour, IArrow
    {
        [SerializeField] private string _sfx = "Archery";
        [Inject] private SoundManager _soundManager;
        private float _speed;
        private int _damage;
        private float _distance;
        private Vector2 _direction;
        private LayerMask _targetLayer;
        private LayerMask _highBarrier;
        private LayerMask _lowBarrier;

        public void Start()
        {
            _highBarrier = LayerMask.GetMask("HighBarrier");
            _lowBarrier = LayerMask.GetMask("LowBarrier");
            _soundManager.StartMusic(_sfx, SoundChannel.Effect);
        }

        public void Setup(ArrowData arrowData, Vector2 diraction)
        {
            _speed = arrowData.speed;
            _damage = arrowData.damage;
            _distance = arrowData.distance;
            _direction = diraction;
            _targetLayer = arrowData.targetLayer;
        }

        public void Update()
        {
            Vector3 p = _direction * (_speed * Time.deltaTime);
            transform.position += p;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            LayerMask otherLayerMask = 1 << other.gameObject.layer;
            if((otherLayerMask | _highBarrier) == _highBarrier || (otherLayerMask | _lowBarrier) == _lowBarrier)
                Destroy(gameObject);
            if ((_targetLayer | otherLayerMask) != _targetLayer) return;
            if (!other.TryGetComponent(out IDamageable damageable)) return;
            damageable.Damage(_damage);
            Destroy(gameObject);
        }
    }
}