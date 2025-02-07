using Agent;
using UnityEngine;

namespace BaseEntity.States
{
    public class WanderByPointState : EntityState
    {
        public WanderByPointData _wanderByPointData;
        private Vector2 _currentPoint;
        private int _currentPointIndex;
        private PhysicAgent _agent;
        
        public override void Start()
        {
            _wanderByPointData = Entity.GetComponent<WanderByPointData>();
            _agent = Entity.GetComponent<PhysicAgent>();
            if (_wanderByPointData == null)
                Debug.LogError("Add WanderByPointData Component to your entity.");
            _currentPoint = _wanderByPointData.WanderPoints[_currentPointIndex];
        }

        public override void Update()
        {
            Vector2 entityPos = Entity.transform.position;
            float distance = (_currentPoint - entityPos).sqrMagnitude;
            if (distance < 0.1f)
                NextPoint();
            
            _agent.Move(_currentPoint);
        }

        private void NextPoint()
        {
            if (_currentPointIndex == _wanderByPointData.WanderPoints.Length - 1)
                _currentPointIndex = 0;
            else
                _currentPointIndex++;
            
            _currentPoint = _wanderByPointData.WanderPoints[_currentPointIndex];
        }
    }
}