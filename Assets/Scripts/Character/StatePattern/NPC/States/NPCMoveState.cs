using UnityEngine;


namespace NPC {
[CreateAssetMenu(menuName = "NPC/States/Move")]
public class MoveState : NPCState {
    public override void Enter()
    {
        //Anim.SetBool("isMoving", true);
        _machine._animator.Play("Walk");
    }

    public override void UpdateState() {
        if (!_machine._agent.pathPending && _machine._agent.remainingDistance <= _machine._agent.stoppingDistance)
        {
            _machine.SwitchState(NPCStateID.Idle);
        }
    }

    public override void OnZoneEnter(NPCTriggerZoneType zone, Collider2D other)
    {
        if (zone == NPCTriggerZoneType.Vision) { _machine.SwitchState(NPCStateID.Chase); }
        //else if (zone == TriggerZoneType.Attack) { _machine.SwitchState(StateID.Attack); }
    }

    public override void Exit()
    {
        _machine._agent.ResetPath();
    }
}
}