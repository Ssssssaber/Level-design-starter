using System.Collections;
using UnityEngine;

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

        [Header("Continuous Check Settings")]
        [Tooltip("How often to check for player visibility while in zone")]
        [SerializeField] private float _checkInterval = 0.1f;

        private int _ownColliderLayer;
        private bool _playerInTrigger;
        private bool _playerWasVisible;
        private Coroutine _visionCheckCoroutine;

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

        void OnEnable()
        {
            if (_dependentOnVision)
            {
                _visionCheckCoroutine = StartCoroutine(VisionCheckRoutine());
            }
        }

        void OnDisable()
        {
            if (_visionCheckCoroutine != null)
            {
                StopCoroutine(_visionCheckCoroutine);
                _visionCheckCoroutine = null;
            }
        }

        private IEnumerator VisionCheckRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_checkInterval);

                if (_playerInTrigger)
                {
                    bool currentlyVisible = IsPlayerVisible();

                    if (currentlyVisible && !_playerWasVisible)
                    {
                        Debug.Log($"{ZoneType} became visible - firing enter");
                        _parentAI.NotifyZoneEnter(ZoneType, GameManager.Instance.Player.GetComponent<Collider2D>());
                    }
                    else if (!currentlyVisible && _playerWasVisible)
                    {
                        Debug.Log($"{ZoneType} became invisible - firing exit");
                        _parentAI.NotifyZoneExit(ZoneType, GameManager.Instance.Player.GetComponent<Collider2D>());
                    }

                    _playerWasVisible = currentlyVisible;
                }
                else
                {
                    _playerWasVisible = false;
                }
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
            if (!IsPlayer(other)) return;
            
            _playerInTrigger = true;
            _playerWasVisible = ShouldNotifyVision();

            Debug.Log($"{ZoneType} entered by {other.gameObject.name}, visible: {_playerWasVisible}");
            
            if (!_dependentOnVision || _playerWasVisible)
            {
                _parentAI.NotifyZoneEnter(ZoneType, other);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!IsPlayer(other)) return;
            
            _playerInTrigger = false;
            _playerWasVisible = false;

            Debug.Log($"{ZoneType} exited by {other.gameObject.name}");
            _parentAI.NotifyZoneExit(ZoneType, other);
        }

        private bool IsPlayer(Collider2D other)
        {
            return GameManager.Instance?.Player != null && other.gameObject == GameManager.Instance.Player.gameObject;
        }

        private bool ShouldNotifyVision()
        {
            if (!_dependentOnVision) return true;
            return IsPlayerVisible();
        }
    }
}
