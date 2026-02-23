using UnityEngine;

namespace Player {
[CreateAssetMenu(menuName = "Player/States/Interact")]
public class InteractState : PlayerState
{
    public override void Enter()
    {
        Debug.Log("Enter IdleState");
        _machine._animator.Play("Interact");
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
        Debug.Log("Exit IdleState");
    }
}
}
