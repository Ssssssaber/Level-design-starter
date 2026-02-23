using System;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Trigger/Attack")]
public class AttackState : State {
    public override void Enter()
    {
        _machine._agent.isStopped = true;
        //Anim.SetTrigger("attack");
        _machine._animator.Play("Attack");
    }

    public override void UpdateState()
    {
    }

    public override void OnZoneExit(TriggerZoneType zone, Collider2D other)
    {
        if (zone == TriggerZoneType.Attack)
        {
            _machine._agent.isStopped = false;
            _machine.SwitchState(StateID.Chase);
        }
    }

    public override void Exit()
    {
        _machine._agent.isStopped = false;
    }
}
