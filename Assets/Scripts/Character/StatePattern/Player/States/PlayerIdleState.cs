using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "Player/States/Idle")]
    public class IdleState : PlayerState
    {
        public override void Enter()
        {
            _machine._animator.Play("Idle");
            _machine._movementSystem.CanMove = false; // Disable movement
        }

        public override void UpdateState()
        {
            // Check if the player starts moving
            if (_machine._movementSystem.CurrentDirection.magnitude > 0.1f)
            {
                _machine.SwitchState(PlayerStateID.Move);
            }
        }

        public override void Exit()
        {
        }
    }
}
