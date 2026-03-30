using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interactable
{
    public class InteractionDetector : MonoBehaviour
    {
        [SerializeField] private GameObject _interactionIcon;
        
        private List<IInteractable> _interactables = new();
        private int _activeIndex = -1;
        private GameObject _interactor;

        private IInteractable CurrentInteractable => _activeIndex >= 0 && _activeIndex < _interactables.Count 
            ? _interactables[_activeIndex] 
            : null;

        void Start()
        {
            _interactionIcon.SetActive(false);
            _interactor = GameManager.Instance.Player.gameObject;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log($"[Interact] ActiveIndex: {_activeIndex}, Count: {_interactables.Count}, Current: {CurrentInteractable}");
                CurrentInteractable?.Interact(_interactor);
            }
        }

        public void OnSwapActiveItems(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SwapActiveItem();
            }
        }

        public void SwapActiveItem()
        {
            if (_interactables.Count > 1)
            {
                _activeIndex = (_activeIndex + 1) % _interactables.Count;
                UpdateIcon();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IInteractable interactable))
            {
                if (interactable.CanInteract(_interactor))
                {
                    _interactables.Add(interactable);
                    
                    if (_activeIndex == -1)
                    {
                        _activeIndex = _interactables.Count - 1;
                    }
                    
                    UpdateIcon();
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IInteractable interactable))
            {
                int removedIndex = _interactables.IndexOf(interactable);
                
                if (removedIndex == -1) return;

                _interactables.RemoveAt(removedIndex);

                if (removedIndex < _activeIndex)
                {
                    _activeIndex--;
                }
                else if (removedIndex == _activeIndex)
                {
                    if (_interactables.Count == 0)
                    {
                        _activeIndex = -1;
                    }
                    else
                    {
                        _activeIndex = _activeIndex % _interactables.Count;
                    }
                }
                
                UpdateIcon();
            }
        }

        private void UpdateIcon()
        {
            bool hasValidInteractable = CurrentInteractable != null && CurrentInteractable.CanInteract(_interactor);
            _interactionIcon.SetActive(hasValidInteractable);
        }
    }
}
