using System;
using DG.Tweening;
using Health;
using UnityEngine;

namespace BaseEntity
{
    public abstract class BaseEntityController : MonoBehaviour, IDamageable
    {
        public Action OnDeath;
        public int Health => healthController.Health;
        
        public int MaxHealth => healthController.MaxHealth;
        
        public Rigidbody2D RigidBody => _rigidBody;
        
        public virtual float Speed => speed;

        public Animator Animator => animator;
        
        public float Radius => radius;

        [SerializeField] protected float radius;
        [SerializeField] protected HealthController healthController;
        [SerializeField] protected DamageFlash damageFlash;
        [SerializeField] protected int maxHealth;
        [SerializeField] protected float rotationSpeed;
        [SerializeField] protected float speed;
        [SerializeField] protected Animator animator;
        
        private Rigidbody2D _rigidBody;

        public virtual void Initialize()
        {
            healthController.Setup(maxHealth, OnDead);
            _rigidBody = GetComponent<Rigidbody2D>();
        }
        
        protected virtual void Awake()
        { }

        protected virtual void Start()
        { }
        
        protected virtual void Update()
        { }

        protected virtual void FixedUpdate()
        { }

        public virtual void Rotate(Vector2 dir, float dt)
        {
            float angle = dir.ToAngle();
            Rotate(angle, dt);
        }

        public void Rotate(float angle, float dt)
        {
            RigidBody.SetRotation(Quaternion.Lerp(
                transform.rotation,
                Quaternion.Euler(new Vector3(0f,0f,angle)),
                rotationSpeed*dt));
        }

        public void ChangeMaxHealth(int maxHealth) => healthController.ChangeMaxHealth(maxHealth);

        public bool Damage(int damage)
        {
            bool isDeath = healthController.Damage(damage);
            damageFlash?.CallDamageFlash();
            return isDeath;
        }

        public void Heal(int health) => healthController.Heal(health);

        public float GetDistanceBetweenEntities(EntityController other)
        {
            float distance = Vector3.Distance(transform.position, other.transform.position);
            return distance - Radius - other.Radius;
        }

        public virtual void OnDead()
        {
            OnDeath?.Invoke();
            DOTween.Kill(gameObject);
            Destroy(gameObject);
        }
    }
}