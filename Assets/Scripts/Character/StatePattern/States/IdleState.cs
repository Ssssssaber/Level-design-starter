using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Trigger/Idle")]
public class IdleState : State {
    public override void Enter()
    {
        Debug.Log("enter idle");
    }

    public override void UpdateState() { }

    public override void OnZoneEnter(TriggerZoneType zone, Collider2D other) {
        if (zone == TriggerZoneType.Vision) _machine.SwitchState(StateID.Chase);
    }

    public override void Exit()
    {
        Debug.Log("exit idle");
    }
}