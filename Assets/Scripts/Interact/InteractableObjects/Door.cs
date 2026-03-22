using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Interactable
{
    public class Door : MonoBehaviour, IInteractable
    {
        [Header("Visuals")]
        [SerializeField] protected Sprite _onState;
        [SerializeField] protected Sprite _offState;
        [SerializeField] protected SpriteRenderer _sprite;

        [Header("Colliders")]
        [SerializeField] BoxCollider2D _collider;


        [Header("Interactable")]
        [Tooltip("If door is stuck player cannot interact with it")]
        [SerializeField] bool _doorStuck = false;

        [Header("Debug")]
        [Tooltip("On Awake() will update its state")]
        [SerializeField] protected bool _toggled = true;

        
        public bool CanInteract(GameObject interactor)
        {
            return  !_doorStuck;
        }

        public virtual void Interact(GameObject interactor)
        {
            if (!CanInteract(interactor)) return;

            _toggled = !_toggled;
            UdpateSprite(_toggled);
            UpdateColliderObstacle(_toggled);
        }

        protected void UpdateColliderObstacle(bool toggled)
        {
            _collider.enabled = toggled;
        }

        protected void UdpateSprite(bool toggled)
        {
            _sprite.sprite = toggled ? _onState : _offState;
        }

        public string GetTag()
        {
            return "Door";
        }
    }
}