using UnityEngine;

namespace Player
{
    public enum PlayerStateID { Idle, Move, Attack, Interact, TakeDamage, Dying }

    public abstract class PlayerState : ScriptableObject, IAnimationEventHandler
    {
        [HideInInspector] protected PlayerStateMachine _machine;
        public virtual void Init(PlayerStateMachine machine)
        {
            _machine = machine;
        }

        public abstract void Enter();
        public abstract void UpdateState();
        public abstract void Exit();

        public virtual void OnAnimationEvent(string animName) { }
    }
}