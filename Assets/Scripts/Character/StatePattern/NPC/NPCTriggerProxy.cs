using UnityEngine;

namespace NPC
{
    public class NPCTriggerProxy : MonoBehaviour
    {
        public NPCTriggerZoneType zoneType;
        private NPCStateMachine _parentAI;

        void Awake()
        {
            _parentAI = GetComponentInParent<NPCStateMachine>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _parentAI.NotifyZoneEnter(zoneType, other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _parentAI.NotifyZoneExit(zoneType, other);
        }
    }
}