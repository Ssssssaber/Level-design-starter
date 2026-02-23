using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "Player/States/Move")]
    public class MoveState : PlayerState
    {
        public override void Enter()
        {
            Debug.Log("Enter MoveState");
            _machine._animator.Play("Walk");
            _machine._movementSystem.CanMove = true; // Enable movement
        }

        public override void UpdateState()
        {
            // Movement is handled by MovementSystem via OnMove input callback
            // Check if the player stops moving
            if (_machine._movementSystem.CurrentDirection.magnitude < 0.1f)
            {
                _machine.SwitchState(PlayerStateID.Idle);
            }
        }

        public override void Exit()
        {
            Debug.Log("Exit MoveState");
            _machine._movementSystem.CanMove = false; // Disable movement
            _machine._movementSystem.SetDirection(Vector2.zero);
            _machine._animator.Play("Idle");
        }
    }
}
