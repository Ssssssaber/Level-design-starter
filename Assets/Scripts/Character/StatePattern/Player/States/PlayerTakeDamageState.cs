using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "Player/States/TakeDamage")]
    public class TakeDamageState : PlayerState
    {
        private PlayerStateID _previousState;

        public override void Enter()
        {
            _previousState = PlayerStateID.Idle;
            Debug.Log($"Enter TakeDamageState. Previous: {_previousState}");
            _machine._movementSystem.CanMove = false; // Disable movement
            _machine._movementSystem.SetDirection(Vector2.zero);
            _machine._animator.Play("TakeDamage");
        }

        // Called by Animation Event at the end of the damage animation
        public void OnDamageAnimationFinished()
        {
            Debug.Log("Damage animation finished!");
            _machine.SwitchState(_previousState);
        }

        public override void UpdateState()
        {
            // No movement during damage
        }

        public override void OnAnimationFinished(string animName)
        {
            if (animName == "TakeDamage")
            {
                Debug.Log("Damage animation finished!");
                _machine.SwitchState(_previousState);
            }
        }

        public override void Exit()
        {
            Debug.Log("Exit TakeDamageState");
        }
    }
}
