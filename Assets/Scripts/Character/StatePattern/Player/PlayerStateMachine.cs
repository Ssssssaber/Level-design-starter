using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerStateMachine : StateMachine
    {
        [System.Serializable]
        public class StateMapping
        {
            public PlayerStateID id;
            public PlayerState stateAsset;
        }

        public class StateManager
        {
            public PlayerStateID CurrentStateID  {get ; private set; }
            public PlayerStateID PrevStateID  {get ; private set; }

            public StateManager(PlayerStateID initState)
            {
                CurrentStateID = initState;
                PrevStateID = initState;
            }

            public void SetState(PlayerStateID newID)
            {
                PrevStateID = CurrentStateID;
                CurrentStateID = newID;
            }
            
        }

        public PlayerStateID InitStateID;
        public List<StateMapping> availableStates;

        private Dictionary<PlayerStateID, PlayerState> _stateCache = new Dictionary<PlayerStateID, PlayerState>();
        public PlayerState _currentState;
        public StateManager _stateManager = new StateManager(PlayerStateID.Idle);
        public Animator _animator;
        public MovementSystem _movementSystem;
        private PlayerInput _playerInput;
        private InputSystem_Actions _actions;
        private SpriteRenderer _sprite;

        // calling this instead of awake
        protected override void Initialize()
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
            _stateManager.SetState(newID);
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

        public override void OnDeath()
        {
            Debug.Log("Player died!");
            SwitchState(PlayerStateID.Dying);
        }

        public override void OnTakeDamage()
        {
            if (_stateManager.CurrentStateID != PlayerStateID.Dying && _stateManager.CurrentStateID != PlayerStateID.TakeDamage)
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