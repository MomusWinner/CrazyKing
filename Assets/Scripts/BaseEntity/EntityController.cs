using Finders;
using UnityEngine;
using UnityEngine.AI;

namespace BaseEntity
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class EntityController : BaseEntityController
    {
        public EntityFinder TargetFinder { get; protected set; }
        public float LookRadius => _lookRadius;

        [SerializeField] private float _lookRadius;
        private NavMeshAgent _agent;

        protected override void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _agent.speed = speed;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            Vector2 dir = _agent.velocity;
            if (dir.sqrMagnitude < 0.5f) return;
            dir = dir.normalized;
            Rotate(dir, Time.fixedDeltaTime);
        }

        public void SetDestination(Vector3 position)
        {
            _agent.SetDestination(position);
        }

        public void Stop()
        {
            _agent.ResetPath();
        }

        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _lookRadius);
            Gizmos.color = Color.cyan;
            if (_agent)
            {
               Gizmos.DrawSphere(_agent.nextPosition, 0.1f);
            }
        }

        public Transform FindTargetInLookRadius()
        {
            return  TargetFinder.FindObjectInLookRadius(transform.position);
        }

        public void SetSpeed(float speed)
        {
            _agent.speed = speed;
            this.speed = speed;
        }
    }
}