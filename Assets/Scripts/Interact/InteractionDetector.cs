using System.ComponentModel.Design.Serialization;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interactable
{
    public class InteractionDetector : MonoBehaviour
    {
        private IInteractable _currentInterractable = null;
        [SerializeField] private GameObject _interactionIcon;
        private GameObject _interactor;

        void Start()
        {
            _interactionIcon.SetActive(false);
            _interactor = GameManager.Instance.Player.gameObject;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _currentInterractable?.Interact(_interactor);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract(_interactor))
            {
                _currentInterractable = interactable;
                _interactionIcon.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IInteractable interactable) && interactable == _currentInterractable)
            {
                _currentInterractable = null;
                _interactionIcon.SetActive(false);
            }
        }
    }
}