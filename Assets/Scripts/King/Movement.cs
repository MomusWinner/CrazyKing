using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private InputActionAsset _actionAsset;
    [SerializeField] private float _rotationSpeed;
    private InputAction _moveAction;
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _moveAction = _actionAsset.FindActionMap("Player").FindAction("Move");
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _moveAction.Enable();
    }

    private void OnEnable()
    {
        _actionAsset.Enable();
    }

    private void OnDisable()
    {
        _actionAsset.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 dir = _moveAction.ReadValue<Vector2>();
        _rigidbody2D.linearVelocity = dir * speed;
        if (dir.magnitude > 0.1f)
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