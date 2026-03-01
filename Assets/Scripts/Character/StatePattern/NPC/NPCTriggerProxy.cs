using UnityEngine;

namespace NPC
{
    public class NPCTriggerProxy : MonoBehaviour
    {
        public NPCTriggerZoneType ZoneType;
        private NPCStateMachine _parentAI;

        void Awake()
        {
            _parentAI = GetComponentInParent<NPCStateMachine>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"{ZoneType} entered by {other.gameObject.name}");
            _parentAI.NotifyZoneEnter(ZoneType, other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log($"{ZoneType} exited by {other.gameObject.name}");
            _parentAI.NotifyZoneExit(ZoneType, other);
        }
    }
}