using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Idle")]
public class IdleState : State {
    public override void Enter()
    {
        Debug.Log("enter idle");
        _machine._animator.Play("Idle");
    }

    public override void UpdateState() { }

    public override void OnZoneEnter(TriggerZoneType zone, Collider2D other)
    {
        if (zone == TriggerZoneType.Vision) { _machine.SwitchState(StateID.Chase); }
        //else if (zone == TriggerZoneType.Attack) { _machine.SwitchState(StateID.Attack); }
    }

    public override void Exit()
    {
        Debug.Log("exit idle");
    }
}