using System;
using Health;
using UnityEngine;

namespace BaseEntity
{
    public abstract class BaseEntityController : MonoBehaviour, IDamageable
    {
        public int Health => _healthController.Health;
        
        public int MaxHealth => _healthController.MaxHealth;
        
        public Rigidbody2D RigidBody => _rigidBody;
        
        public float Speed => _speed;
        
        public Animator Animator => _animator;
        
        public float Radius => _radius;

        [SerializeField] private float _radius;
        [SerializeField] private HealthUIData _healthData;
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _speed;
        [SerializeField] private Animator _animator;
        private Rigidbody2D _rigidBody;
        private HealthController _healthController;

        protected virtual void Awake()
        { }

        protected virtual void Start()
        {
            _healthController = new HealthController(_maxHealth, _healthData, OnDead);
            _rigidBody = GetComponent<Rigidbody2D>();
        }
        
        protected virtual void Update()
        {
        }

        protected virtual void FixedUpdate()
        {
        }

        public virtual void Rotate(Vector2 dir, float t)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            RigidBody.SetRotation(Quaternion.Lerp(
                transform.rotation,
                Quaternion.Euler(new Vector3(0f,0f,angle)),
                _rotationSpeed*Time.fixedDeltaTime));
        }

        public void ChangeMaxHealth(int maxHealth) => _healthController.ChangeMaxHealth(maxHealth);

        public bool Damage(int damage) => _healthController.Damage(damage);

        public void Heal(int health) => _healthController.Heal(health);

        public float GetDistanceBetweenEntities(EntityController other)
        {
            float distance = Vector3.Distance(transform.position, other.transform.position);
            return distance - Radius - other.Radius;
        }

        public virtual void OnDead() => Destroy(gameObject);
    }
}