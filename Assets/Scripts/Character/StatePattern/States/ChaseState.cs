using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Trigger/Chase")]
public class ChaseState : State {
    public override void Enter()
    {
        //Anim.SetBool("isMoving", true);
        _machine._animator.Play("Walk");
    }

    public override void UpdateState()
    {
        _machine._agent.SetDestination(_machine.Player.position);
    }

    public override void OnZoneEnter(TriggerZoneType zone, Collider2D other)
    {
        if (zone == TriggerZoneType.Attack) _machine.SwitchState(StateID.Attack);
    }

    public override void OnZoneExit(TriggerZoneType zone, Collider2D other)
    {
        if (zone != TriggerZoneType.Vision) return;

        _machine._agent.SetDestination(_machine._initialPosiiton);
        _machine.SwitchState(StateID.Move);
    }

    public override void Exit()
    {
    }
}
