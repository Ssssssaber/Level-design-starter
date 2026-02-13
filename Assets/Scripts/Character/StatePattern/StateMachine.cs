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
    private Animator _animator;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

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
        _currentState?.UpdateState();
    }

    public void SwitchState(StateID newID)
    {
        _currentState?.Exit();
        _currentState = _stateCache[newID];
        _currentState.Enter();
    }

    public void NotifyZoneEnter(TriggerZoneType zone, Collider2D other)
    {
        _currentState?.OnZoneEnter(zone, other);
    }

    public void NotifyZoneExit(TriggerZoneType zone, Collider2D other)
    {
        _currentState?.OnZoneExit(zone, other);
    }
}
