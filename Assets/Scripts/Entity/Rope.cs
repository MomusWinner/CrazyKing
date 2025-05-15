using UnityEngine;

namespace Entity
{
    public class Rope : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Transform _target;

        public void Update()
        {
            if (_target != null)
                _lineRenderer.SetPosition(1, _target.position - transform.position );
            else
                _lineRenderer.enabled = false;
        }
    }
}