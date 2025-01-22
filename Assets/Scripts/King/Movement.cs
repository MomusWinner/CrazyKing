using Controllers;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public bool FreezeMovement { get; set; }
    public bool FreezeRotation { get; set; }
    
    [SerializeField] private float speed;
    [SerializeField] private float _rotationSpeed;
    [Inject] private InputManager _inputManager;
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 dir = _inputManager.MoveDirection;
        
        if (!FreezeMovement)
            _rigidbody2D.linearVelocity = dir * speed;
        
        if (!FreezeRotation && dir.magnitude > 0.1f)
            SetRotation(dir);
    }

    private void SetRotation(Vector2 dir)
    {
        float angle = dir.ToAngle();
        var rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(new Vector3(0f,0f,angle)),
            _rotationSpeed*Time.fixedDeltaTime);
        _rigidbody2D.SetRotation(rotation);
    }
}