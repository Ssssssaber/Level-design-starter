using System.Collections.Generic;
using UnityEngine;
using GameObjectsSound;
using UnityEngine.InputSystem;

namespace Interactable
{
    public class InteractionDetector : MonoBehaviour
    {
        [SerializeField] private GameObject _interactionIcon;
        [SerializeField] private Vector2 _iconOffset = new Vector2(0, 1.5f);
        
        private List<IInteractable> _interactables = new();
        private int _activeIndex = -1;
        private GameObject _interactor;
        private Transform _iconTransform;

        private IInteractable CurrentInteractable => _activeIndex >= 0 && _activeIndex < _interactables.Count 
            ? _interactables[_activeIndex] 
            : null;

        private MonoBehaviour CurrentInteractableComponent => CurrentInteractable as MonoBehaviour;

        void Start()
        {
            _interactionIcon.SetActive(false);
            _interactor = GameManager.Instance.Player.gameObject;
            _iconTransform = _interactionIcon.transform;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log($"[Interact] ActiveIndex: {_activeIndex}, Count: {_interactables.Count}, Current: {CurrentInteractable}");
                CurrentInteractable?.Interact(_interactor);
                // Play interaction sound if player has a sound profile attached
                var profile = _interactor?.GetComponent<SoundProfileContainer>()?.GetProfile();
                if (profile != null)
                {
                    GameManager.Instance.FXSoundPlayer.PlaySound(SoundID.Interact, profile, _interactor.transform);
                }
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
            if (_interactionIcon == null)
            {
                Debug.Log($"Interaction icon on {gameObject.name} is null");
                return;
            }

            bool hasValidInteractable = CurrentInteractable != null && CurrentInteractable.CanInteract(_interactor);
            
            if (hasValidInteractable && CurrentInteractableComponent != null)
            {
                _interactionIcon.transform.SetParent(null);
                Vector2 targetPos = (Vector2)CurrentInteractableComponent.transform.position + _iconOffset;
                _iconTransform.position = targetPos;
                _interactionIcon.SetActive(true);
            }
            else
            {
                _interactionIcon.SetActive(false);
                _interactionIcon.transform.SetParent(_interactor.transform);
                _interactionIcon.transform.localPosition = Vector3.zero;
            }
        }
    }
}
