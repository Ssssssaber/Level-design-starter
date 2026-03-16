using NavMeshPlus.Extensions;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(menuName = "NPC/States/Dying")]
    public class DyingState : NPCState
    {
        private float _deathTimer = 3f; // Time to remain "dead" before respawning/removal

        public override void Enter()
        {
            Debug.Log("Enter DyingState");
            _machine._agent.isStopped = true;
            _machine._animator.Play("Death");
            _machine.SetEnabledColliders(false);
        }

        public override void UpdateState()
        {

        }

        public override void Exit()
        {
            Debug.Log("Exit DyingState");
        }
    }
}
