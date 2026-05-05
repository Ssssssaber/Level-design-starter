using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using GameObjectsSound;
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
            // Play door open/close sound using interactor's profile
            var profile = interactor?.GetComponent<GameObjectsSound.SoundProfileContainer>()?.GetProfile();
            if (profile != null)
            {
                var soundId = _toggled ? SoundID.Door_Open : SoundID.Door_Close;
                GameManager.Instance.FXSoundPlayer.PlaySound(soundId, profile, transform);
            }
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
