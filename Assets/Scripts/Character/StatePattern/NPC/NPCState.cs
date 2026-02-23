
using Player;
using UnityEngine;

namespace NPC
{
    public enum NPCStateID { Idle, Move, Chase, Attack, TakeDamage, Dying }
    public enum NPCTriggerZoneType { Vision, Attack }

    public abstract class NPCState : ScriptableObject, IAnimationEventHandler
    {
        [HideInInspector] protected NPCStateMachine _machine;
        public virtual void Init(NPCStateMachine machine)
        {
            _machine = machine;
        }

        public abstract void Enter();
        public abstract void UpdateState();
        public abstract void Exit();

        public virtual void OnZoneEnter(NPCTriggerZoneType zone, Collider2D other) { }
        public virtual void OnZoneExit(NPCTriggerZoneType zone, Collider2D other) { }

        public virtual void OnAnimationFinished(string animName) { }
    }
}