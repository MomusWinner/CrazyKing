using BaseEntity.States;
using Controllers.SoundManager;
using Health;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

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

        public void Start()
        {
            _soundManager.StartMusic(_sfx, SoundChannel.Effect, Random.Range(0.85f, 1.15f));
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
            if ((_targetLayer| (1 << other.gameObject.layer)) != _targetLayer) return;
            if (!other.TryGetComponent(out IDamageable damageable)) return;
            damageable.Damage(_damage);
            Destroy(this.gameObject);
        }
    }
}