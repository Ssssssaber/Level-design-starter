using UnityEngine;

namespace Interactable
{
    public interface IInteractable
    {
        public string GetTag();
        public void Interact(GameObject interactor);
        public bool CanInteract(GameObject interactor);
    }
}

