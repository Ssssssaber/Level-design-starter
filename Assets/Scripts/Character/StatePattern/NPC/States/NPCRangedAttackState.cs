using System;
using RangedAttack;
using GameObjectsSound;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(menuName = "NPC/States/Trigger/RangedAttack")]
    public class RangedAttackState : NPCState
    {
        bool _targetInRange = false;
        private RangedAttackManager _rangedAttack;

        public override void Init(NPCStateMachine machine)
        {
            base.Init(machine);
            if (_rangedAttack == null) GetRangedAttackComponent();
        }

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
            if (zone == NPCTriggerZoneType.RangedAttack)
            {
                _targetInRange = true;
            }
        }

        public override void OnZoneExit(NPCTriggerZoneType zone, Collider2D other)
        {
            if (zone == NPCTriggerZoneType.RangedAttack)
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
                case "LaunchProjectile":
                    {
                        FacePlayer();
                        GameManager.Instance.FXSoundPlayer.PlaySound(SoundID.Attack, _machine._soundManager.GetProfile(), _machine.transform);
                        _rangedAttack.LaunchProjectlie(GameManager.Instance.Player.transform.position);
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

        private void GetRangedAttackComponent()
        {
            _rangedAttack = _machine.gameObject.GetComponent<RangedAttackManager>();
            if (_rangedAttack == null)
            {
                Debug.LogError($"State machine for {_machine.gameObject.name} doesnt have ranged attack manager!");
            }
        }

        public override void Exit()
        {
            _machine._agent.isStopped = false;
        }
    }
}
