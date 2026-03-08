using TMPro.EditorUtilities;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using Movement;

namespace Player
{

    public class PlayerController : MonoBehaviour
    {
        private MovementSystem _movementSystem;
        private PlayerInput _playerInput;
        private InputSystem_Actions _actions;
        private SpriteRenderer _sprite;

        void Awake()
        {
            _movementSystem = GetComponent<MovementSystem>();
            _playerInput = GetComponent<PlayerInput>();
            _sprite = GetComponent<SpriteRenderer>();

            _actions = new InputSystem_Actions();

            _playerInput.actions = _actions.asset;

            _actions.Player.Move.performed += OnMove;
            _actions.Player.Move.canceled += OnMove;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>().normalized;

            SpriteUtils.SetFlipX(transform, direction.x < 0);
            _movementSystem.SetDirection(direction);
        }
    }
}
