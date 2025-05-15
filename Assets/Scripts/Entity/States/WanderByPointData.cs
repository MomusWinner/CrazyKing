using TriInspector;
using UnityEngine;

namespace Entity.States
{
    public class WanderByPointData : MonoBehaviour
    {
#if UNITY_EDITOR
        [ShowIf(nameof(_showPoints))]
        [SerializeField]
        private float _pointWidth = 0.2f;
        [ShowIf(nameof(_showPoints))]
        [SerializeField] 
        private Color _pointColor = Color.white;
        [SerializeField]
        private bool _showPoints;
#endif
        public Vector2[] WanderPoints;

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if (!_showPoints) return;
            Gizmos.color = _pointColor;
            for (int i = 0; i < WanderPoints.Length; i++)
                Gizmos.DrawSphere(WanderPoints[i], _pointWidth + _pointWidth * 0.15f * i);
            Gizmos.DrawLine(WanderPoints[0], WanderPoints[^1]);
            for (int i = 1; i < WanderPoints.Length; i++)
                Gizmos.DrawLine(WanderPoints[i - 1], WanderPoints[i]);
        }
#endif
    }
}