using UnityEngine;
using UnityEngine.InputSystem;

namespace MineAndRefact.Core
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerControls _controls;
        private Vector2 _direction;
        private bool _isHoldJoystick;

        public Vector2 Direction => _direction;
        public bool IsHoldJoystick => _isHoldJoystick;


        private void Awake()
        {
            _controls = new PlayerControls();
            _controls.Player.Movement.performed += OnMovement;
            _controls.Player.JoystickActive.started += OnJoystickHold;
            _controls.Player.JoystickActive.canceled += OnJoystickRealeased;
        }

        private void OnEnable()
        {
            _controls.Enable();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }


        public void OnMovement(InputAction.CallbackContext context)
        {
            _direction = context.ReadValue<Vector2>();
        }

        public void OnJoystickHold(InputAction.CallbackContext context)
        {
            _isHoldJoystick = context.control.IsPressed();
        }

        public void OnJoystickRealeased(InputAction.CallbackContext context)
        {
            _isHoldJoystick = context.control.IsPressed();
        }
    }
}