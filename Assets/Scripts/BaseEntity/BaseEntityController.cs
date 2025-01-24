using DG.Tweening;
using Health;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BaseEntity
{
    public abstract class BaseEntityController : MonoBehaviour, IDamageable
    {
        public int Health => _healthController.Health;
        
        public int MaxHealth => _healthController.MaxHealth;
        
        public Rigidbody2D RigidBody => _rigidBody;
        
        public virtual float Speed => speed;

        public Animator Animator => animator;
        
        public float Radius => radius;

        [SerializeField] protected float radius;
        [SerializeField] protected HealthUIData healthData;
        [SerializeField] protected int maxHealth;
        [SerializeField] protected float rotationSpeed;
        [SerializeField] protected float speed;
        [SerializeField] protected Animator animator;
        [SerializeField] protected SpriteRenderer body;
        private Rigidbody2D _rigidBody;
        private HealthController _healthController;

        public virtual void Initialize()
        {
            _healthController = new HealthController(maxHealth, healthData, OnDead);
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

        public virtual void Rotate(Vector2 dir, float t)
        {
            float angle = dir.ToAngle();
            RigidBody.SetRotation(Quaternion.Lerp(
                transform.rotation,
                Quaternion.Euler(new Vector3(0f,0f,angle)),
                rotationSpeed*Time.fixedDeltaTime));
        }

        public void ChangeMaxHealth(int maxHealth) => _healthController.ChangeMaxHealth(maxHealth);

        public bool Damage(int damage)
        {
            bool isDeath = _healthController.Damage(damage);
            if (!isDeath)
            { 
                Sequence sequence = DOTween.Sequence().SetId(gameObject);
                sequence.Append(body.DOColor(new Color(1f, 0f, 0f, 1f), 0.2f));
                sequence.Append(body.DOColor(new Color(1f, 1f, 1f, 1f), 0.2f));               
            }
            return isDeath;
        }

        public void Heal(int health) => _healthController.Heal(health);

        public float GetDistanceBetweenEntities(EntityController other)
        {
            float distance = Vector3.Distance(transform.position, other.transform.position);
            return distance - Radius - other.Radius;
        }

        public virtual void OnDead()
        {
            DOTween.Kill(gameObject);
            Destroy(gameObject);
        }
    }
}