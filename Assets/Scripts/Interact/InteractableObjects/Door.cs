using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Interactable
{
    public class Door : MonoBehaviour, IInteractable
    {
        [Header("Visuals")]
        [SerializeField] private Sprite _onState;
        [SerializeField] private Sprite _offState;
        [SerializeField] private SpriteRenderer _sprite;

        [Header("Colliders")]
        [SerializeField] BoxCollider2D _collider;


        [Header("Interactable")]
        [Tooltip("If door is stuck player cannot interact with it")]
        [SerializeField] bool _doorStuck = false;

        [Header("Debug")]
        [Tooltip("On Awake() will update its state")]
        [SerializeField] private bool _toggled = true;

        
        public bool CanInteract()
        {
            return  !_doorStuck;
        }

        public void Interact()
        {
            if (!CanInteract()) return;

            _toggled = !_toggled;
            UdpateSprite(_toggled);
            UpdateColliderObstacle(_toggled);
        }

        private void UpdateColliderObstacle(bool toggled)
        {
            _collider.enabled = toggled;
        }

        private void UdpateSprite(bool toggled)
        {
            _sprite.sprite = toggled ? _onState : _offState;
        }
    }
}