using Effect;
using Entity.King;
using Entity.States;
using Health;
using UnityEngine;

namespace Entity
{
    [RequireComponent(typeof(SpriteColorizer))]
    public class Chicken : EntityController, IThrowable
    {
        [SerializeField] private float _waitTime = 2f;
        [SerializeField] private float _wanderRadius = 4f;
        [SerializeField] private string _kickParticle;
        [SerializeField] private float _explosionForce = 10f;
        [SerializeField] private string _explosionParticle;
        [SerializeField] private float _explosionTime = 4f;
        [SerializeField] private int _explosionDamage = 50;
        [SerializeField] private float _explosionRadius = 5f;
        [SerializeField] private float _flashSpeed = 3f;
    
        private SpriteColorizer _spriteColorizer;
        private WanderState _wanderState;
        private bool _isExploding;
        private float _explosionTimeLeft;
    
        private float _timeLeft;
    
        protected override void Start()
        {
            base.Start();
            Initialize();
        
            _spriteColorizer = GetComponent<SpriteColorizer>();
            _wanderState = new WanderState();
            _wanderState.WanderRadius = _wanderRadius;
            _wanderState.WaitTime = _waitTime;
            _wanderState.Setup(this);
            _wanderState.Start();
            _spriteColorizer.SetFlashColor(Color.red);
        }

        protected override void Update()
        {
            base.Update();
            if (!_isExploding)
                _wanderState.Update();
            else
            {
                _explosionTimeLeft -= Time.deltaTime;
                _timeLeft += Time.deltaTime * (_flashSpeed + (6f * _flashSpeed  * (1 - _explosionTimeLeft / _explosionTime)));
                float flashAmount = (1 + Mathf.Sin(_timeLeft)) / 2f;
                _spriteColorizer.SetFlashAmount(flashAmount);
                if (_explosionTimeLeft <= 0)
                {
                    Explode();
                }
            }
        }

        public void Explode()
        {
            GameObject explosionObj = Resources.Load<GameObject>(_explosionParticle);
            Instantiate(explosionObj, transform.position, Quaternion.identity);
        
            var entities = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);

            Destroy(gameObject);
            foreach (var entity in entities)
            {
                if (entity.gameObject == gameObject) continue;
                if (entity.TryGetComponent(out IDamageable damageable))
                {
                    bool objectDestroyed = damageable.Damage(_explosionDamage);
                    if (!objectDestroyed && entity.TryGetComponent(out IThrowable throwable))
                    {
                        throwable.Throw(entity.transform.position - transform.position, _explosionForce);
                    }
                }
                else if (entity.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.AddForce((entity.transform.position - transform.position) * _explosionForce, ForceMode2D.Impulse);
                }
            }
        }

        public override bool Damage(int damage)
        {
            _isExploding = true;
            Explode();
            return true;
        }

        public void Throw(Vector2 direction, float force)
        {
            if (!_isExploding)
            {
                _explosionTimeLeft = _explosionTime;
                _wanderState.Dispose();
            }
            
            RigidBody.AddForce(direction * force, ForceMode2D.Impulse);
            _isExploding = true;
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
    }
}