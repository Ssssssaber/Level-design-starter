using UnityEngine;

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
                _machine.SwitchState(PlayerStateID.Idle);
            }
        }

        public override void Exit()
        {
        }
    }
}
