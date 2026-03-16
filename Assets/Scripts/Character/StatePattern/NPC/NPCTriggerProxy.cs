using UnityEngine;
using UnityEngine.InputSystem;

namespace NPC
{
    public class NPCTriggerProxy : MonoBehaviour
    {
        public NPCTriggerZoneType ZoneType;
        private NPCStateMachine _parentAI;
        private CircleCollider2D _collider;

        void Awake()
        {
            _parentAI = GetComponentInParent<NPCStateMachine>();
            _collider = GetComponent<CircleCollider2D>();
        }

        public bool IsPlayerIn()
        {
            Vector2 playerPos = GameManager.Instance.Player.transform.position;
            return _collider.OverlapPoint(playerPos);
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