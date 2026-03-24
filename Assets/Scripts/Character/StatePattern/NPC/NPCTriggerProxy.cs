using UnityEngine;
using UnityEngine.InputSystem;

namespace NPC
{
    public class NPCTriggerProxy : MonoBehaviour
    {
        public NPCTriggerZoneType ZoneType;
        
        [Header("Vision Settings")]
        [Tooltip("If true, will also check raycast to verify player is visible")]
        [SerializeField] private bool _dependentOnVision = true;
        
        [Tooltip("Layers that block vision (e.g. walls)")]
        [SerializeField] private LayerMask _visionBlockingLayers = Physics2D.DefaultRaycastLayers;

        private int _ownColliderLayer;

        private NPCStateMachine _parentAI;
        private CircleCollider2D _collider;

        void Awake()
        {
            _parentAI = GetComponentInParent<NPCStateMachine>();
            _collider = GetComponent<CircleCollider2D>();
            _ownColliderLayer = _collider.gameObject.layer;

            if (_dependentOnVision && _visionBlockingLayers == 0)
            {
                _visionBlockingLayers = LayerMask.GetMask("Default");
            }
        }

        public bool IsPlayerIn()
        {
            if (GameManager.Instance?.Player == null) return false;

            Vector2 playerPos = GameManager.Instance.Player.transform.position;
            bool inTrigger = _collider.OverlapPoint(playerPos);

            if (_dependentOnVision && inTrigger)
            {
                return IsPlayerVisible();
            }

            return inTrigger;
        }

        private bool IsPlayerVisible()
        {
            if (GameManager.Instance?.Player == null) return false;

            GameObject player = GameManager.Instance.Player.gameObject;
            Vector2 from = transform.position;
            Vector2 to = player.transform.position;

            int layerMask = _visionBlockingLayers & ~(1 << _ownColliderLayer);

            RaycastHit2D hit = Physics2D.Linecast(from, to, layerMask);

            if (hit.collider == null)
            {
                return true;
            }

            return hit.collider.gameObject == player;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!ShouldNotify(other)) return;
            
            Debug.Log($"{ZoneType} entered by {other.gameObject.name}");
            _parentAI.NotifyZoneEnter(ZoneType, other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!ShouldNotify(other)) return;
            
            Debug.Log($"{ZoneType} exited by {other.gameObject.name}");
            _parentAI.NotifyZoneExit(ZoneType, other);
        }

        private bool ShouldNotify(Collider2D other)
        {
            if (GameManager.Instance?.Player == null) return true;
            if (other.gameObject != GameManager.Instance.Player.gameObject) return false;
            if (_dependentOnVision) return IsPlayerVisible();
            return true;
        }
    }
}