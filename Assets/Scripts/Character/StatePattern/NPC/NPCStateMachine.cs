using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC {
public class NPCStateMachine : MonoBehaviour
{
    [System.Serializable]
    public class StateMapping {
        public NPCStateID id;
        public NPCState stateAsset;
    }

    public Transform Player;
    public NPCStateID InitStateID;
    public List<StateMapping> availableStates;

    private Dictionary<NPCStateID, NPCState> _stateCache = new Dictionary<NPCStateID, NPCState>(); 
    public NPCState _currentState;
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
        if (!_agent.isStopped)
        {
            _sprite.flipX = _agent.velocity.x < 0;
        }

        _currentState?.UpdateState();
    }

    public void SwitchState(NPCStateID newID)
    {
        //Debug.LogWarning($"Switch state: {newID} from {_currentState}");
        _currentState?.Exit();
        _currentState = _stateCache[newID];
        _currentState.Enter();
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
}
}