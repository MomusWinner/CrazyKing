using System;
using UnityEngine;
using UnityEngine.AI;

namespace BaseEntity
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class EntityController : BaseEntityController
    {
        public float LookRadius => _lookRadius;

        [SerializeField] private float _lookRadius;
        private NavMeshAgent _agent;

        protected void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _agent.speed = Speed;
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

        public Transform FindObjectInLookRadius(LayerMask targetMask, LayerMask ignoreMask)
        {
            Vector3 entityPos = transform.position;
            var targets = Physics2D.OverlapCircleAll(
                transform.position,
                LookRadius,
                targetMask);

            Array.Sort(targets, (collider2D1, collider2D2) =>
            {
                float dist1 = (collider2D1.transform.position - entityPos).sqrMagnitude;
                float dist2 = (collider2D2.transform.position - entityPos).sqrMagnitude;
                if (dist1 < dist2)
                    return -1;
                if (dist1 > dist2)
                    return 1;

                return 0;
            });

            foreach (var target in targets)
            {
                RaycastHit2D hit = Physics2D.Raycast(entityPos,
                    target.transform.position - entityPos,
                    LookRadius,  ~ignoreMask);

                if (hit.collider != null && targetMask == (targetMask | (1 << hit.collider.gameObject.layer)))
                    return hit.collider.transform;
            }

            return null;    
        }
    }
}