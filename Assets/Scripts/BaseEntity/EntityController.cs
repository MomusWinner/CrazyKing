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
            _agent.updatePosition = false;
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

        public void Move(Vector3 position)
        {
            _agent.SetDestination(position);
            Vector3 direction = (_agent.steeringTarget - transform.position).normalized; // Get direction towards steering target
            RigidBody.linearVelocity = direction * speed; // Set Rigidbody velocity based on direction

            // Update NavMeshAgent's next position to match Rigidbody's position
            _agent.nextPosition = RigidBody.position;

            // Optionally handle rotation
            // if (RigidBody.velocity != Vector2.zero) {
            //     Quaternion targetRotation = Quaternion.LookRotation(RigidBody.velocity);
            //     transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            // }
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