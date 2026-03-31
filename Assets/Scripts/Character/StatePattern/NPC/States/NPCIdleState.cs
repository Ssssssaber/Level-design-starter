using UnityEngine;


namespace NPC
{
    [CreateAssetMenu(menuName = "NPC/States/Idle")]
    public class IdleState : NPCState
    {
        public override void Enter()
        {
            _machine._animator.Play("Idle");
        }

        public override void UpdateState() { }

        public override void OnZoneEnter(NPCTriggerZoneType zone, Collider2D other)
        {
            if (zone == NPCTriggerZoneType.RangedAttack) { _machine.SwitchState(NPCStateID.RangedAttack); }
            else if (zone == NPCTriggerZoneType.Attack) { _machine.SwitchState(NPCStateID.Attack); }
            else if (zone == NPCTriggerZoneType.Vision) { _machine.SwitchState(NPCStateID.Chase); }
        }

        public override void Exit()
        {
        }
    }
}