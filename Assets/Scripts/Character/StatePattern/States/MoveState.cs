using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Move")]
public class MoveState : State {
    public override void Enter()
    {
        //Anim.SetBool("isMoving", true);
        _machine._animator.Play("Walk");
    }

    public override void UpdateState() {
        if (!_machine._agent.pathPending && _machine._agent.remainingDistance <= _machine._agent.stoppingDistance)
        {
            _machine.SwitchState(StateID.Idle);
        }
    }

    public override void OnZoneEnter(TriggerZoneType zone, Collider2D other)
    {
        if (zone == TriggerZoneType.Vision) { _machine.SwitchState(StateID.Chase); }
        //else if (zone == TriggerZoneType.Attack) { _machine.SwitchState(StateID.Attack); }
    }

    public override void Exit()
    {
        _machine._agent.ResetPath();
    }
}
