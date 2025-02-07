
using System.Collections;
using Agent;
using UnityEngine;

namespace BaseEntity.States
{
    public class WanderState : EntityState
    {
        private readonly float _wanderRadius = 4f;
        private Vector2? _target = null;
        private PhysicAgent _agent;
        private IEnumerator _wanderCoroutine;
        
        public override void Start()
        {
            _agent = Entity.GetComponent<PhysicAgent>();
            _wanderCoroutine = Wander();
            Entity.StartCoroutine(_wanderCoroutine);
        }

        public override void Update()
        {
            if (_target.HasValue)
            {
                _agent.Move(_target.Value);
            }
            else
            {
                _agent.Stop();
            }
        }

        public override void Dispose()
        {
            if (Entity != null)
            {
                Entity.StopCoroutine(_wanderCoroutine);
            }
        }

        public IEnumerator Wander()
        {
            while (true)
            {
                _target = GetWanderPosition();
                yield return new WaitForSeconds(1.4f);
            }
        }

        public Vector2? GetWanderPosition()
        {
            int tries = 5;
            for (int i = 0; i < tries; i++)
            {
                Vector2 entityPos = Entity.transform.position;
                Vector2 randPos = Random.insideUnitCircle * _wanderRadius + entityPos;
                
                if (!PointIsFree(randPos))
                    continue;
                
                
                return randPos;
            }

            return null;
        }

        public bool PointIsFree(Vector2 pos)
        {
            Collider2D[] collisions = Physics2D.OverlapCircleAll(pos, Entity.Radius);
            return collisions.Length == 0;
        }
    }
}