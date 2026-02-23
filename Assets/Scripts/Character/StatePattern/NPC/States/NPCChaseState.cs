using UnityEngine;


namespace NPC {
[CreateAssetMenu(menuName = "NPC/States/Trigger/Chase")]
public class ChaseState : NPCState {
    public override void Enter()
    {
        //Anim.SetBool("isMoving", true);
        _machine._animator.Play("Walk");
    }

    public override void UpdateState()
    {
        _machine._agent.SetDestination(_machine.Player.position);
    }

    public override void OnZoneEnter(NPCTriggerZoneType zone, Collider2D other)
    {
        if (zone == NPCTriggerZoneType.Attack) _machine.SwitchState(NPCStateID.Attack);
    }

    public override void OnZoneExit(NPCTriggerZoneType zone, Collider2D other)
    {
        if (zone != NPCTriggerZoneType.Vision) return;

        _machine._agent.SetDestination(_machine._initialPosiiton);
        _machine.SwitchState(NPCStateID.Move);
    }

    public override void Exit()
    {
    }
}
}