using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateMachine : MonoBehaviour
{
    [System.Serializable]
    public class StateMapping {
        public StateID id;
        public State stateAsset;
    }

    public Transform Player;
    public StateID InitStateID;
    public List<StateMapping> availableStates;

    private Dictionary<StateID, State> _stateCache = new Dictionary<StateID, State>(); 
    public State _currentState;
    public NavMeshAgent _agent;
    public Animator _animator;
    private SpriteRenderer _sprite;
    public Vector2 _initialPosiiton;

    private void Awake()
    {
        _initialPosiiton = transform.position;
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _agent = GetComponent<NavMeshAgent>();

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        foreach (var mapping in availableStates) {
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
        _sprite.flipX = _agent.velocity.x < 0;

        _currentState?.UpdateState();
    }

    public void SwitchState(StateID newID)
    {
        Debug.LogWarning($"Switch state: {newID} from {_currentState}");
        _currentState?.Exit();
        _currentState = _stateCache[newID];
        _currentState.Enter();
    }

    public void NotifyZoneEnter(TriggerZoneType zone, Collider2D other)
    {
        Debug.LogWarning($"Zone {zone} ENTERED by {other.gameObject.name}");
        _currentState?.OnZoneEnter(zone, other);
    }

    public void NotifyZoneExit(TriggerZoneType zone, Collider2D other)
    {
        Debug.LogWarning($"Zone {zone} EXITED by {other.gameObject.name}");
        _currentState?.OnZoneExit(zone, other);
    }
}
