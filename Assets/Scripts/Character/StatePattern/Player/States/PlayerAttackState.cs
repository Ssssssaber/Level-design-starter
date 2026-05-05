using UnityEngine;
using GameObjectsSound;

namespace Player
{
    [CreateAssetMenu(menuName = "Player/States/Attack")]
    public class AttackState : PlayerState
    {
        public override void Enter()
        {
            _machine._animator.Play("Attack");
            _machine._movementSystem.CanMove = false;
            _machine._movementSystem.SetDirection(Vector2.zero);
        }

        public override void UpdateState() { }

        public override void OnAnimationEvent(string animName)
        {
            if (animName == "Attack")
            {
                // Play attack sound using player's sound profile
                if (_machine._soundManager != null)
                {
                    GameManager.Instance.FXSoundPlayer.PlaySound(SoundID.Attack, _machine._soundManager.GetProfile(), _machine.transform);
                }
                _machine.SwitchState(PlayerStateID.Idle);
            }
        }

        public override void Exit()
        {
        }
    }
}
