using UnityEngine;
using UnityEngine.AI;

namespace Agent
{
    [RequireComponent(typeof(Rigidbody2D), typeof(NavMeshAgent))]
    public class PhysicAgent : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 10;
        public float Speed { get; set; } = 3f;
        public float RotationSpeed { get; set; }
        private NavMeshAgent _agent;
        private Rigidbody2D _rigidbody;
        
        public void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _agent.updatePosition = false;
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            
            RotationSpeed = _rotationSpeed;
        }

        public void Move(Vector3 position)
        {
            _agent.SetDestination(position);
            Vector3 direction = (_agent.steeringTarget - transform.position).normalized; // Get direction towards steering target
            _rigidbody.linearVelocity = direction * Speed; // Set Rigidbody velocity based on direction

            // Update NavMeshAgent's next position to match Rigidbody's position
            _agent.nextPosition = _rigidbody.position;

            // Optionally handle rotation
            // if (RigidBody.velocity != Vector2.zero) {
            //     Quaternion targetRotation = Quaternion.LookRotation(RigidBody.velocity);
            //     transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            // }
        }

        protected void FixedUpdate()
        {
            Vector2 dir = _agent.velocity;
            if (dir.sqrMagnitude < 0.5f) return;
            dir = dir.normalized;
            _rigidbody.RotateLerp(RotationSpeed, dir.ToAngle(), Time.fixedDeltaTime);
        }

        public void Stop()
        {
            _agent.ResetPath();
        }
    }
}