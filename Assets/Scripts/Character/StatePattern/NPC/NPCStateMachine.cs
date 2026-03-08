using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;

namespace NPC
{
    public class NPCStateMachine : StateMachine
    {
        [System.Serializable]
        public class StateMapping
        {
            public NPCStateID id;
            public NPCState stateAsset;
        }

        public class StateManager
        {
            public NPCStateID CurrentStateID  {get ; private set; }
            public NPCStateID PrevStateID  {get ; private set; }

            public StateManager(NPCStateID initState)
            {
                CurrentStateID = initState;
                PrevStateID = initState;
            }

            public void SetState(NPCStateID newID)
            {
                PrevStateID = CurrentStateID;
                CurrentStateID = newID;
            }
            
        }

        public Transform Player;
        public NPCStateID InitStateID;
        public List<StateMapping> availableStates;

        private Dictionary<NPCStateID, NPCState> _stateCache = new Dictionary<NPCStateID, NPCState>();
        public NPCState _currentState;
        public StateManager _stateManager = new StateManager(NPCStateID.Idle);
        public NavMeshAgent _agent;
        public Animator _animator;
        public SpriteRenderer _sprite;
        public Vector2 _initialPosiiton;

        // Calls when awake in base class
        protected override void Initialize()
        {
            _initialPosiiton = transform.position;
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
            _agent = GetComponent<NavMeshAgent>();

            _agent.updateRotation = false;
            _agent.updateUpAxis = false;

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
            if (!_agent.isStopped)
            {
                SpriteUtils.SetFlipX(transform, _agent.velocity.x < 0);
            }

            _currentState?.UpdateState();
        }

        public void SwitchState(NPCStateID newID)
        {
            //Debug.LogWarning($"Switch state: {newID} from {_currentState}");
            _currentState?.Exit();
            _currentState = _stateCache[newID];
            _currentState.Enter();
            _stateManager.SetState(newID);
        }

        public void NotifyZoneEnter(NPCTriggerZoneType zone, Collider2D other)
        {
            //Debug.LogWarning($"Zone {zone} ENTERED by {other.gameObject.name}");
            _currentState?.OnZoneEnter(zone, other);
        }

        public void NotifyZoneExit(NPCTriggerZoneType zone, Collider2D other)
        {
            //Debug.LogWarning($"Zone {zone} EXITED by {other.gameObject.name}");
            _currentState?.OnZoneExit(zone, other);
        }

        public void OnAnimationEvent(string eventName)
        {
            Debug.Log($"Animation Event: {eventName}");
            if (_currentState is IAnimationEventHandler handler)
            {
                handler.OnAnimationEvent(eventName);
            }
        }

        public override void OnDeath()
        {
            Debug.Log("NPC died!");
            SwitchState(NPCStateID.Dying);
        }

        public override void OnTakeDamage()
        {
            if (_stateManager.CurrentStateID != NPCStateID.Dying && _stateManager.CurrentStateID != NPCStateID.TakeDamage)
            {
                SwitchState(NPCStateID.TakeDamage);
            }
        }
    }
}