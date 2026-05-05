using System.Runtime.InteropServices.WindowsRuntime;
using GameObjectsSound;
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
            // Play door open/close sound using interactor's profile
            var profile = interactor?.GetComponent<GameObjectsSound.SoundProfileContainer>()?.GetProfile();
            if (profile != null)
            {
                var soundId = _opened ? SoundID.Door_Open : SoundID.Door_Close;
                GameManager.Instance.FXSoundPlayer.PlaySound(soundId, profile, transform);
            }
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

        public void OnInteractSound(GameObject interactor)
        {
            var localProfile = GetComponent<GameObjectsSound.SoundProfileContainer>()?.GetProfile();
            var profile = localProfile ?? interactor?.GetComponent<GameObjectsSound.SoundProfileContainer>()?.GetProfile();
            if (profile != null)
            {
                var soundId = _opened ? SoundID.Door_Open : SoundID.Door_Close;
                GameManager.Instance.FXSoundPlayer.PlaySound(soundId, profile, transform);
            }
        }
    }
}
