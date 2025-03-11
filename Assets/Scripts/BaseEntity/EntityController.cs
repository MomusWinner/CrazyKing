using Agent;
using Enemy.FSM;
using Finders;
using UnityEngine;
using VContainer;

namespace BaseEntity
{
    [RequireComponent(typeof(PhysicAgent))]
    public abstract class EntityController : BaseEntityController
    {
        public EntityFinder TargetFinder { get; protected set; }

        public float LookRadius
        {
            get => _lookRadius;
            set => _lookRadius = value;
        }
        

        [SerializeField] private float _lookRadius;
        private PhysicAgent _agent;

        protected override void Awake()
        {
            _agent = GetComponent<PhysicAgent>();
            _agent.Speed = speed;
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _lookRadius);
            Gizmos.color = Color.cyan;
        }

        public Transform FindTargetInLookRadius()
        {
            return  TargetFinder.FindObjectInLookRadius(transform.position, _lookRadius);
        }

        public void SetSpeed(float speed)
        {
            _agent.Speed = speed;
            this.speed = speed;
        }
    }
}