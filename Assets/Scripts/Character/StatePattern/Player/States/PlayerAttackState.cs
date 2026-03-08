using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "Player/States/Attack")]
    public class AttackState : PlayerState
    {
        public override void Enter()
        {
            Debug.Log("Enter AttackState");
            _machine._animator.Play("Attack");
            _machine._movementSystem.CanMove = false;
            _machine._movementSystem.SetDirection(Vector2.zero);
        }

        public override void UpdateState() { }

        public override void OnAnimationEvent(string animName)
        {
            if (animName == "Attack")
            {
                Debug.Log("Attack animation finished!");
                _machine.SwitchState(PlayerStateID.Idle);
            }
        }

        public override void Exit()
        {
            Debug.Log("Exit AttackState");
        }
    }
}
