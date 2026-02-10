using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private MovementSystem _movementSystem;
    private PlayerInput _playerInput;
    private InputSystem_Actions _actions;

    void Awake()
    {
        _movementSystem = GetComponent<MovementSystem>();
        _playerInput = GetComponent<PlayerInput>();

        _actions = new InputSystem_Actions();

        _playerInput.actions = _actions.asset;

        _actions.Player.Move.performed += OnMove;
        _actions.Player.Move.canceled += OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _movementSystem.SetDirection(
            context.ReadValue<Vector2>().normalized
        );
    }
}
