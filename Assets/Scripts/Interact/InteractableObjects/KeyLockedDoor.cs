using System.Runtime.InteropServices.WindowsRuntime;
using Inventory;
using Unity.VisualScripting;
using UnityEngine;

namespace Interactable
{
    public class KeyLockedDoor : MonoBehaviour, IInteractable
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
        [SerializeField] protected bool _opened = true;

        
        public bool CanInteract(GameObject interactor)
        {
            bool canInteract = !_opened;
            if (!canInteract) return false;

            if (interactor.TryGetComponent(out InventoryManager inventory))
            {
                IInteractable key = inventory.FindFirstItemWithTag("Key");
                if (key == null)
                {
                    Debug.LogWarning($"{interactor.name} has no key in inventory");
                }

                return key != null;
            }
            else
            {
                Debug.LogWarning($"{interactor.name} has no inventory manager");
            }

            return false;
        }

        public virtual void Interact(GameObject interactor)
        {
            if (!CanInteract(interactor)) return;

            if (interactor.TryGetComponent(out InventoryManager inventory))
            {
                bool haskey = inventory.TryUseFirstItemWithTag("Key", out IInteractable key);
                if (!haskey)
                {
                    Debug.LogError("no key");
                    return;
                }
            }

            _opened = true;
            UdpateSprite(_opened);
            UpdateColliderObstacle(_opened);
            Debug.LogWarning("Door opened");
        }

        protected void UpdateColliderObstacle(bool opened)
        {
            _collider.enabled = !opened;
        }

        private void UdpateSprite(bool opened)
        {
            _sprite.sprite = !opened ? _onState : _offState;
        }

        public string GetTag()
        {
            return "KeyDoor";
        }
    }
}