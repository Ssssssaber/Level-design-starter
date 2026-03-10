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

        void Start()
        {
            _interactionIcon.SetActive(false);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            Debug.LogWarning("kKEKEKEK");
            if (context.performed)
            {
            Debug.LogWarning("sdfsdf");
                _currentInterractable?.Interact();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
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