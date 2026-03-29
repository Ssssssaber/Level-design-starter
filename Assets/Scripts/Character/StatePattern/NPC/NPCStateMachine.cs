using System.Collections.Generic;
using System.Data;
using Sound;
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

        [Header("State pattern")]
        public NPCStateID InitStateID;
        public List<StateMapping> availableStates;
        public NPCState _currentState;

        [Header("Detection zones")]
        public NPCTriggerProxy _visionZone;
        public NPCTriggerProxy _attackZone;
        public NPCTriggerProxy _rangedAttackZone;

        [Header("Other references")]
        public SpriteRenderer _sprite;

        private Dictionary<NPCStateID, NPCState> _stateCache = new Dictionary<NPCStateID, NPCState>();
        [HideInInspector] public StateManager _stateManager = new StateManager(NPCStateID.Idle);
        [HideInInspector] public NavMeshAgent _agent;
        [HideInInspector] public SoundProfileContainer _soundManager;
        [HideInInspector] public Animator _animator;
        [HideInInspector] public Vector2 _initialPosiiton;

        // Calls when awake in base class
        protected override void Initialize()
        {
            _initialPosiiton = transform.position;
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
            _agent = GetComponent<NavMeshAgent>();
            _soundManager = GetComponent<SoundProfileContainer>();

            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _agent.radius = 0.3f;
            _agent.avoidancePriority = Random.Range(0, 100);
            _agent.autoBraking = true;
            _agent.autoRepath = true;

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
            if (HasState(NPCStateID.TakeDamage) &&
                _stateManager.CurrentStateID != NPCStateID.Dying &&
                _stateManager.CurrentStateID != NPCStateID.TakeDamage)
            {
                SwitchState(NPCStateID.TakeDamage);
            }
        }

        private bool HasState(NPCStateID targetStateID)
        {
            foreach (var state in availableStates)
            {
                if (state.id == targetStateID)
                    return true; 
            }

            return false;
        }

        public void AllStateCheck()
        {
            if (_rangedAttackZone != null &&
                _rangedAttackZone.IsPlayerIn())
            {
                SwitchState(NPCStateID.RangedAttack);
            }
            else if (_attackZone != null &&
                     _attackZone.IsPlayerIn())
            {
                SwitchState(NPCStateID.Attack);
            }
            else if (_visionZone != null &&
                     _visionZone.IsPlayerIn())
            {
                SwitchState(NPCStateID.Chase);
            }
            else
            {
                _agent.SetDestination(_initialPosiiton);
                SwitchState(NPCStateID.Move);
            }
        }
        
        public void SetEnabledColliders(bool value)
        {
            var allColliders = GetComponentsInChildren<Collider2D>();
            foreach (Collider2D collider in allColliders)
            {
                collider.enabled = value;
            }
        }
    }
}