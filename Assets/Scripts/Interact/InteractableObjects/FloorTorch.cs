using PuzzleSystem;
using UnityEngine;

namespace Interactable
{
    public class FloorTorch : MonoBehaviour, IInteractable, IPuzzleElement
    {
        [Header("Visuals")]
        [SerializeField] private Sprite _onState;
        [SerializeField] private Sprite _offState;
        [SerializeField] private SpriteRenderer _sprite;

        [Header("Debug")]
        [Tooltip("On Awake() will update its state")]
        [SerializeField] private bool _toggled = true;

        public PuzzleState CurrentState => _toggled ? PuzzleState.On : PuzzleState.Off;

        private void Awake()
        {
            UdpateSprite(_toggled);
        }

        public void SetState(PuzzleState state)
        {
            _toggled = state == PuzzleState.On;
            UdpateSprite(_toggled);
        }

        public bool CanInteract(GameObject interactor)
        {
            return true;
        }

        public void Interact(GameObject interactor)
        {
            if (!CanInteract(interactor)) return;

            _toggled = !_toggled;
            UdpateSprite(_toggled);
            PuzzleEvents.NotifyStateChanged(this);
        }

        private void UdpateSprite(bool toggled)
        {
            _sprite.sprite = toggled ? _onState : _offState;
        }

        public string GetTag()
        {
            return "FloorTorch";
        }
    }
}