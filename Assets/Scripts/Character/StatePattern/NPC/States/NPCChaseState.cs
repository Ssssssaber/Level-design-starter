using UnityEngine;


namespace NPC
{
    [CreateAssetMenu(menuName = "NPC/States/Trigger/Chase")]
    public class ChaseState : NPCState
    {
        public override void Enter()
        {
            //Anim.SetBool("isMoving", true);
            _machine._animator.Play("Walk");
        }

        public override void UpdateState()
        {
            _machine._agent.SetDestination(GameManager.Instance.Player.transform.position);
            
            // Continuously check if player is still visible while chasing
            if (_machine._visionZone != null && !_machine._visionZone.IsPlayerIn())
            {
                // Player is no longer visible or in vision zone - return to patrol
                _machine._agent.SetDestination(_machine._initialPosiiton);
                _machine.SwitchState(NPCStateID.Move);
            }
        }

        public override void OnZoneEnter(NPCTriggerZoneType zone, Collider2D other)
        {
            if (zone == NPCTriggerZoneType.Attack) _machine.SwitchState(NPCStateID.Attack);
            if (zone == NPCTriggerZoneType.RangedAttack) _machine.SwitchState(NPCStateID.RangedAttack);
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