using System.Collections.Generic;
using UnityEngine;

namespace King
{
    public class Kick : MonoBehaviour
    {
        [SerializeField] private float _startScale = 0.3f; 
        [SerializeField] private float _endScale = 1f;
        [SerializeField] private float _kickDistance = 1f;
        [SerializeField] private float _speed = 3f;
        [SerializeField] private float _force = 2f;
        
        private bool _initialized;
        private Vector2 _direction;
        private float _distanceTraveled;
        private List<GameObject> _kickedObjects = new();
        
        public void Setup(Vector2 direction)
        {
            _direction = direction;
            transform.position.Scale(new Vector2(0.1f, 0.1f));
            _initialized = true;
            transform.Rotate(new Vector3(0, 0, direction.ToAngle()));
        }

        public void Update()
        {
            if (!_initialized) return;

            if (_distanceTraveled > _kickDistance)
            {
                Destroy(gameObject);
                return;
            }
            
            _distanceTraveled += Time.deltaTime * _speed;
            transform.position += transform.right * (Time.deltaTime * _speed);
            transform.localScale = Vector3.one * Mathf.Lerp(_startScale, _endScale, _distanceTraveled / _kickDistance);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject == gameObject) return;
            
            if (_kickedObjects.Contains(other.gameObject)) return;
            _kickedObjects.Add(other.gameObject);
            IThrowable throwable = other.GetComponent<IThrowable>();
            if (throwable != null)
            {
                throwable.Throw(_direction, _force);
                return;
            }

            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(_force * _direction, ForceMode2D.Impulse);
            }
        }
    }

    public interface IThrowable
    {
        void Throw(Vector2 direction, float force);
    }
}