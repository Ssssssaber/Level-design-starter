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
        }

        public override void UpdateState()
        {
            _deathTimer -= Time.deltaTime;
            if (_deathTimer <= 0f)
            {
                // Respawn or remove the player
                Debug.Log("NPC respawn/removal logic here");
                
            }
        }

        public override void Exit()
        {
            Debug.Log("Exit DyingState");
        }
    }
}
