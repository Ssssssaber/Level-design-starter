using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(menuName = "NPC/States/TakeDamage")]
    public class TakeDamageState : NPCState
    {
        private NPCStateID _previousState;

        public override void Enter()
        {
            _previousState = _machine._stateManager.PrevStateID;
            Debug.Log($"Enter TakeDamageState. Previous: {_previousState}");
            _machine._agent.isStopped = true;
            _machine._animator.Play("TakeDamage");
        }


        public override void UpdateState()
        {
            // No movement during damage
        }

        public override void OnAnimationEvent(string animName)
        {
            if (animName == "TakeDamage")
            {
                Debug.Log("Damage animation finished!");
                _machine.SwitchState(_previousState);
                _machine._agent.isStopped = false;
            }
        }

        public override void Exit()
        {
            Debug.Log("Exit TakeDamageState");
        }
    }
}
