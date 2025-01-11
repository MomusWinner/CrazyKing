using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace King
{
    public interface IPoint
    {
        Vector2 GetPosition();
    }
    
    class Point: IPoint
    {
        public readonly Transform centerTransform;
        public Vector2 centerOffset;

        public Point(Transform centerTransform, Vector2 centerOffset)
        {
            this.centerTransform = centerTransform;
            this.centerOffset = centerOffset;
        }

        /// <returns>Global position</returns>
        public Vector2 GetPosition()
        {
            if (!centerTransform)
                return Vector2.zero;
            
            Vector2 centerPos = new Vector2
            (
                centerTransform.transform.position.x,
                centerTransform.transform.position.y
            );
            return centerPos + centerOffset;
        }
    }
    
    
    public class CirclePointController: MonoBehaviour
    {
        private float _pointRadius = 2;
        private int _count = 5;
        private List<IPoint> _freePoints = new();
        private List<IPoint> _busyPoints = new();
        
        public void Awake()
        {
            GeneratePoints();
        }

        public void GeneratePoints()
        {
            for (int i = 0; i < _count; i++)
            {
                float angle = Mathf.Lerp(0, 360, (float)i / _count);
                Vector2 position = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                position *= _pointRadius;
                CreatePoint(position);
            }
        }

        private void CreatePoint(Vector2 position)
        {
            var newPoint = new Point(transform, position);
            _freePoints.Add(newPoint);
        }

        public bool TryGetFreePoint(out IPoint point)
        {
            point = null;
            if (_freePoints.Count == 0)
                return false;

            point = _freePoints.First(); // TODO remove
            _busyPoints.Add(point);
            _freePoints.Remove(point);
            return true;
        }

        public void ReturnPoint(IPoint point)
        {
            bool removed = _busyPoints.Remove(point);
            if (removed)
                _freePoints.Add(point);
        }

        
        void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.green;
            foreach (var point in _freePoints)
            {
                Gizmos.DrawSphere(point.GetPosition(), 0.1f);
            }
            Gizmos.color = Color.red;
            foreach (var point in _busyPoints)
            {
                Gizmos.DrawSphere(point.GetPosition(), 0.1f);
            }
        }
    }
}
