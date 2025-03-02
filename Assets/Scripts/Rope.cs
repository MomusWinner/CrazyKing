using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _target;

    public void Update()
    {
        _lineRenderer.SetPosition(1, _target.position - transform.position );
    }
}