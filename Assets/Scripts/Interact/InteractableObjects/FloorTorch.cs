using UnityEngine;

namespace Interactable
{
    public class FloorTorch : MonoBehaviour, IInteractable
    {
        [Header("Visuals")]
        [SerializeField] private Sprite _onState;
        [SerializeField] private Sprite _offState;
        [SerializeField] private SpriteRenderer _sprite;

        [Header("Debug")]
        [Tooltip("On Awake() will update its state")]
        [SerializeField] private bool _toggled = true;

        private void Awake()
        {
            UdpateSprite(_toggled);
        }

        public bool CanInteract()
        {
            return true;
        }

        public void Interact()
        {
            if (!CanInteract()) return;

            _toggled = !_toggled;
            UdpateSprite(_toggled);
        }

        private void UdpateSprite(bool toggled)
        {
            _sprite.sprite = toggled ? _onState : _offState;
        }
    }
}