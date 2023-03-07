using UnityEngine;
using UnityEngine.InputSystem;

namespace MineAndRefact.Core
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        private Vector2 _direction;
        private bool _isHoldJoystick;

        public Vector2 Direction => _direction;
        public bool IsHoldJoystick => _isHoldJoystick;


        private PlayerInput _playerInput;
        public PlayerInput CachedPlayerInput
        {
            get
            {
                if (_playerInput == null)
                    _playerInput = GetComponent<PlayerInput>();
                return _playerInput;
            }
        }


        public void OnMovement(InputAction.CallbackContext context)
        {
            _isHoldJoystick = context.action.IsPressed();
            _direction = context.ReadValue<Vector2>();   
        }
    }
}