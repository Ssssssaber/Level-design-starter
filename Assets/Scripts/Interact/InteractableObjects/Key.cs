using Inventory;
using UnityEngine;

namespace Interactable
{
    public class Key : MonoBehaviour, IInteractable
    {
        public bool CanInteract(GameObject interactor)
        {
            return true;
        }

        public void Interact(GameObject interactor)
        {
            if (interactor.TryGetComponent(out InventoryManager inventory))
            {
                if (inventory.AddInteractable(this))
                {
                    Destroy(transform.parent.gameObject);
                }
                else
                {
                    Debug.LogWarning("key was not added to inventory");
                }
            }
            else
            {
                Debug.LogError($"{interactor.name} has no inventory manger");
            }
        }

        public string GetTag()
        {
            return "Key";
        }
    }
}