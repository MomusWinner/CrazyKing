using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Controllers
{
    public class InputManager : IStartable, ITickable, IDisposable
    {
        public bool MouseAvailable => Mouse.current != null;
        public Vector2 MousePosition => Mouse.current.position.ReadValue();
        public Vector2 MoveDirection => _moveAction.ReadValue<Vector2>();
        public Action OnAttack;
        public Action OnKick;
        
        [Inject] private InputActionAsset _actionAsset;
        private InputAction _moveAction;
        private InputAction _attackAction;
        private InputAction _kickAction;
        
        public void Start()
        {
            _actionAsset.Enable();
            _moveAction = _actionAsset.FindActionMap("Player").FindAction("Move");
            _attackAction = _actionAsset.FindActionMap("Player").FindAction("Attack");
            _kickAction = _actionAsset.FindActionMap("Player").FindAction("Kick");
            
            _attackAction.performed += _ => OnAttack?.Invoke();
            _kickAction.performed += _ => OnKick?.Invoke();
        }

        public void Tick()
        {
        }

        public void Dispose()
        {
            _actionAsset.Disable();
        }
    }
}