using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Trigger/Chase")]
public class ChaseState : State {
    public override void Enter()
    {
        //Anim.SetBool("isMoving", true);
        Debug.Log("enter chase");
    }

    public override void UpdateState() {
        _machine._agent.SetDestination(_machine.Player.position);
    }

    public override void OnZoneEnter(TriggerZoneType zone, Collider2D other) {
        if (zone == TriggerZoneType.Attack) _machine.SwitchState(StateID.Attack);
    }

    public override void OnZoneExit(TriggerZoneType zone, Collider2D other) {
        if (zone == TriggerZoneType.Vision) _machine.SwitchState(StateID.Idle);
    }

    public override void Exit()
    {
        Debug.Log("exit chase");
    }
}
