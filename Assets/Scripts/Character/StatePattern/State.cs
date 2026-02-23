
using UnityEngine;
using UnityEngine.AI;

public enum StateID { Idle, Move, Chase, Attack }
public enum TriggerZoneType { Vision, Attack }

public abstract class State : ScriptableObject
{
    [HideInInspector] protected StateMachine _machine;
    public virtual void Init(StateMachine machine)
    {
        _machine = machine;
    }

    public abstract void Enter();
    public abstract void UpdateState();
    public abstract void Exit();

    public virtual void OnZoneEnter(TriggerZoneType zone, Collider2D other) { }
    public virtual void OnZoneExit(TriggerZoneType zone, Collider2D other) { }
}
