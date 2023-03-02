using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private bool _hasMovement;
    private Vector2 _direction;

    public PlayerInput CachedPlayerInput
    {
        get
        {
            if (_playerInput == null)
                _playerInput = GetComponent<PlayerInput>();
            return _playerInput;
        }
    }

    public Vector2 Direction => _direction;


    public void OnMovement(InputAction.CallbackContext context)
    {
        _direction = context.ReadValue<Vector2>();
    }
}
