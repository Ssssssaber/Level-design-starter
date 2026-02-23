using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [System.Serializable]
        public class StateMapping
        {
            public PlayerStateID id;
            public PlayerState stateAsset;
        }

        public PlayerStateID InitStateID;
        public List<StateMapping> availableStates;

        private Dictionary<PlayerStateID, PlayerState> _stateCache = new Dictionary<PlayerStateID, PlayerState>();
        public PlayerState _currentState;
        public PlayerStateID _currentStateID;
        public Animator _animator;
        public MovementSystem _movementSystem;
        private PlayerInput _playerInput;
        private InputSystem_Actions _actions;
        private SpriteRenderer _sprite;

        private void Awake()
        {
            _movementSystem = GetComponent<MovementSystem>();
            _playerInput = GetComponent<PlayerInput>();
            _sprite = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();

            _actions = new InputSystem_Actions();

            _playerInput.actions = _actions.asset;

            _actions.Player.Move.performed += OnMove;
            _actions.Player.Move.canceled += OnMove;
            _actions.Player.Attack.performed += _ => SwitchState(PlayerStateID.Attack);

            foreach (var mapping in availableStates)
            {
                var instance = Instantiate(mapping.stateAsset);
                instance.Init(this);
                _stateCache[mapping.id] = instance;
            }
        }

        private void Start()
        {
            SwitchState(InitStateID);
        }

        private void Update()
        {
            _currentState?.UpdateState();
        }

        public void SwitchState(PlayerStateID newID)
        {
            Debug.LogWarning($"PLAYER Switch state: {newID} from {_currentState}");
            _currentState?.Exit();
            _currentState = _stateCache[newID];
            _currentStateID = newID;
            _currentState.Enter();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>().normalized;
            if (_movementSystem.CanMove)
            {
                _sprite.flipX = direction.x < 0;
            }
            _movementSystem.SetDirection(direction); // Update direction, but movement is controlled by the state
        }

        public void TakeDamage()
        {
            if (_currentStateID != PlayerStateID.Dying && _currentStateID != PlayerStateID.TakeDamage)
            {
                SwitchState(PlayerStateID.TakeDamage);
            }
        }

        public void OnAnimationEvent(string eventName)
        {
            Debug.Log($"Animation Event: {eventName}");
            if (_currentState is IAnimationEventHandler handler)
            {
                handler.OnAnimationFinished(eventName);
            }
        }
    }
}