using System;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(menuName = "NPC/States/Trigger/Attack")]
    public class AttackState : NPCState
    {
        bool _targetInRange = false;
        public override void Enter()
        {
            _targetInRange = true;
            _machine._agent.isStopped = true;
            //Anim.SetTrigger("attack");
            _machine._animator.Play("Attack");
        }

        public override void UpdateState()
        {
        }

        public override void OnZoneEnter(NPCTriggerZoneType zone, Collider2D other)
        {
            if (zone == NPCTriggerZoneType.Attack)
            {
                _targetInRange = true;
            }
        }

        public override void OnZoneExit(NPCTriggerZoneType zone, Collider2D other)
        {
            if (zone == NPCTriggerZoneType.Attack)
            {
                _targetInRange = false;
            }
        }

        public override void OnAnimationStarted(string animName)
        {
            if (animName == "Attack")
            {
                FacePlayer();
            }
        }

        public override void OnAnimationFinished(string animName)
        {
            if (animName == "Attack" && !_targetInRange)
            {
                ExitAttackState();
            }
        }

        private void ExitAttackState()
        {
            Debug.LogError("exit attack)");
            _machine._agent.isStopped = false;
            _machine.SwitchState(NPCStateID.Chase);
        }

        private void FacePlayer()
        {
            SpriteUtils.SetFlipX(
                _machine.transform,
                (_machine.Player.transform.position.x - _machine.transform.position.x) < 0
            );
        }

        public override void Exit()
        {
            _machine._agent.isStopped = false;
        }
    }
}
