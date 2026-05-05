using UnityEngine;
using GameObjectsSound;

namespace NPC
{
    [CreateAssetMenu(menuName = "NPC/States/TakeDamage")]
    public class TakeDamageState : NPCState
    {
        public override void Enter()
        {
            _machine._agent.isStopped = true;
            _machine._animator.Play("TakeDamage");
            // Play take-damage sound
            if (_machine._soundManager != null)
            {
                GameManager.Instance.FXSoundPlayer.PlaySound(SoundID.TakeDamage, _machine._soundManager.GetProfile(), _machine.transform);
            }
        }


        public override void UpdateState()
        {
            // No movement during damage
        }

        public override void OnAnimationEvent(string animName)
        {
            if (animName == "TakeDamage")
            {
                _machine._agent.isStopped = false;
                _machine.AllStateCheck();
            }
        }

        public override void Exit()
        {
        }
    }
}
