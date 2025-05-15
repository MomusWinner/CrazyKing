using System.Collections;
using Agent;
using UnityEngine;

namespace Entity.States
{
    public class WanderState : EntityState
    {
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");
        public float WanderRadius { get; set; } = 4f;
        public float WaitTime { get; set; } = 2f;
        public float ErrorTime { get; set; } = 20f;
        
        private Vector2? _target = null;
        private PhysicAgent _agent;
        private IEnumerator _wanderCoroutine;
        
        public override void Start()
        {
            _agent = Entity.GetComponent<PhysicAgent>();
            _wanderCoroutine = Wander();
            Entity.StartCoroutine(_wanderCoroutine);
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
            float wanderTime = 0;
            while (true)
            {
                if (wanderTime > 10f){
                    _target = GetWanderPosition();
                    wanderTime = 0;
                }

                Vector2 agentPos = _agent.transform.position;

                if (!_target.HasValue)
                {
                    _target = GetWanderPosition();
                    wanderTime = 0;
                }
                else
                {
                    if ((agentPos - _target.Value).magnitude < 1f)
                    {
                        _agent.Stop();
                        Entity.Animator.SetBool(IsRunning, false);
                        yield return new WaitForSeconds(WaitTime);
                        _target = GetWanderPosition();
                        wanderTime = 0;
                    }
                }

                if (_target.HasValue)
                {
                    _agent.Move(_target.Value);
                    Entity.Animator.SetBool(IsRunning, true);
                }
                wanderTime += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
        }

        public Vector2? GetWanderPosition()
        {
            int tries = 5;
            for (int i = 0; i < tries; i++)
            {
                Vector2 entityPos = Entity.transform.position;
                Vector2 randPos = Random.insideUnitCircle * WanderRadius + entityPos;
                
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