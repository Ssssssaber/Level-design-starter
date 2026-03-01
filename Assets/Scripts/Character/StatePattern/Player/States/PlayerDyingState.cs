using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "Player/States/Dying")]
    public class DyingState : PlayerState
    {
        private float _deathTimer = 3f; // Time to remain "dead" before respawning/removal

        public override void Enter()
        {
            Debug.Log("Enter DyingState");
            _machine._movementSystem.CanMove = false; // Disable movement
            _machine._movementSystem.SetDirection(Vector2.zero);
            _machine._animator.Play("Death");
        }

        public override void UpdateState()
        {
            _deathTimer -= Time.deltaTime;
            if (_deathTimer <= 0f)
            {
                // Respawn or remove the player
                Debug.Log("Player respawn/removal logic here");
                
            }
        }

        public override void Exit()
        {
            Debug.Log("Exit DyingState");
        }
    }
}
