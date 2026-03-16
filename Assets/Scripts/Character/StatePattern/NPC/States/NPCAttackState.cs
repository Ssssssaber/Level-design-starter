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
        public override void OnAnimationEvent(string animName)
        {
            switch (animName)
            {
                case "AttackStarted":
                    {
                        FacePlayer();
                        break;
                    }
                case "AttackFinished":
                    {
                        if (!_targetInRange) ExitAttackState();
                        break;
                    }
            }
        }

        private void ExitAttackState()
        {
            _machine._agent.isStopped = false;
            _machine.SwitchState(NPCStateID.Chase);
        }

        private void FacePlayer()
        {
            SpriteUtils.SetFlipX(
                _machine.transform,
                (GameManager.Instance.Player.transform.position.x - _machine.transform.position.x) < 0
            );
        }

        public override void Exit()
        {
            _machine._agent.isStopped = false;
        }
    }
}
