using System;
using UnityEngine;

namespace NPC {
[CreateAssetMenu(menuName = "AI/States/Trigger/Attack")]
public class AttackState : NPCState {
    public override void Enter()
    {
        _machine._agent.isStopped = true;
        //Anim.SetTrigger("attack");
        _machine._animator.Play("Attack");
    }

    public override void UpdateState()
    {
    }

    public override void OnZoneExit(NPCTriggerZoneType zone, Collider2D other)
    {
        if (zone == NPCTriggerZoneType.Attack)
        {
            _machine._agent.isStopped = false;
            _machine.SwitchState(NPCStateID.Chase);
        }
    }

    public override void Exit()
    {
        _machine._agent.isStopped = false;
    }
}    
}
