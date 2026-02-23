using UnityEngine;


namespace NPC {
[CreateAssetMenu(menuName = "NPC/States/Idle")]
public class IdleState : NPCState {
    public override void Enter()
    {
        Debug.Log("enter idle");
        _machine._animator.Play("Idle");
    }

    public override void UpdateState() { }

    public override void OnZoneEnter(NPCTriggerZoneType zone, Collider2D other)
    {
        if (zone == NPCTriggerZoneType.Vision) { _machine.SwitchState(NPCStateID.Chase); }
        //else if (zone == TriggerZoneType.Attack) { _machine.SwitchState(StateID.Attack); }
    }

    public override void Exit()
    {
        Debug.Log("exit idle");
    }
}
}