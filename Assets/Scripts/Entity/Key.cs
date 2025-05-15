using System.Collections;
using Entity.King;
using UnityEngine;

namespace Entity
{
    public class Key : MonoBehaviour
    {
        public bool IsTaken { get; private set; }
        [SerializeField] private float _followingRadius;
        [SerializeField] private float _speed;
        
        private Transform _kingTransform;
        private bool _isMovingToDoor;

        private void Update()
        {
            if (_kingTransform == null || _isMovingToDoor) 
            {
                return;
            }

            float distance = (_kingTransform.position - transform.position).magnitude;
            if (distance < _followingRadius)
            {
                return;
            }

            transform.position = Vector3.Lerp(
                transform.position,
                _kingTransform.position,
                _speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (LayerMasks.King != 1 << other.gameObject.layer)
            {
                return;
            }

            if (!other.TryGetComponent(out KingController king))
            {
                return;
            }
            
            _kingTransform = other.transform;
            IsTaken = true;
        }

        public IEnumerator MoveToDoor(Vector3 doorPosition)
        {
            _isMovingToDoor = true;
            while (true)
            {
                if ((doorPosition - transform.position).sqrMagnitude < 0.01f)
                {
                    break;
                }
                transform.position = Vector3.Lerp(
                    transform.position,
                    doorPosition,
                    _speed * 2 * Time.deltaTime);
                yield return null;
            }
        }
    }
}