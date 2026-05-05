using UnityEngine;

namespace Interactable
{
    public interface IInteractable
    {
        public string GetTag();
        public void Interact(GameObject interactor);
        public bool CanInteract(GameObject interactor);
        // Sound hook: play interact sound when player interacts with this object
        public void OnInteractSound(GameObject interactor);
    }
}

