using UnityEngine;

namespace Interactable
{
    public class Note : MonoBehaviour, IInteractable
    {
        public bool CanInteract()
        {
            return true;
        }

        public void Interact()
        {
            throw new System.NotImplementedException();
        }
    }
}
