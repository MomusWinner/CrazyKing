using System.Collections.Generic;
using System.Linq;
using Servant;
using UnityEngine;
using VContainer;

namespace King
{
    public interface IPoint
    {
        int Id {get;}
        Vector2 GetPosition();
    }
    
    class Point: IPoint
    {
        public int Id { get; }
        public readonly Transform centerTransform;
        public Vector2 centerOffset;

        public Point(int id, Transform centerTransform, Vector2 centerOffset)
        {
            Id = id;
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
        private List<IPoint> _freePoints = new();
        private List<IPoint> _busyPoints = new();
        private List<IPoint> Points => _freePoints.Concat(_busyPoints).ToList();
        [Inject] private ServantsSO servantsSO;
        
        public void Awake()
        {
            GeneratePoints();
        }

        public void GeneratePoints()
        {
            foreach (var ring in servantsSO.Rings)
            {
                for (int i = 0; i < ring.quantity; i++)
                {
                    float angle = Mathf.Lerp(0, 360, (float)i / ring.quantity);
                    angle += ring.angleOffset;
                    Vector2 position = MathUtils.AngleToVector2(angle) * ring.radius;
                    CreatePoint(position);
                }
            }
        }

        private void CreatePoint(Vector2 position)
        {
            int id = 0;
            if (_freePoints.Any())
                id = _freePoints.Last().Id + 1;
            var newPoint = new Point(id, transform, position);
            _freePoints.Add(newPoint);
        }

        public bool TryGetPoint(int id, out IPoint point)
        {
            point = null;
            if (_freePoints.Count == 0)
                return false;

            point = _freePoints.Find(p => p.Id == id);
            if (point == null) return false;
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

        public bool TryGetPointPosition(int id, out Vector2 position)
        {
            position = Vector2.zero;
            IPoint point = Points.Find(p => p.Id == id);
            if (point is null)
                return false;
            position = point.GetPosition();
            return true;
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
