using UnityEngine;

namespace Interactable
{
    public class Note : MonoBehaviour, IInteractable
    {
        [SerializeField] bool _opened = false;
        [SerializeField] GameObject _noteGameObject;
        
        private void Awake()
        {
            UpdateNote(_opened);
        }

        public bool CanInteract(GameObject interactor)
        {
            return true;
        }

        public string GetTag()
        {
            return "Note";
        }

        public void Interact(GameObject interactor)
        {
            if (!CanInteract(interactor))
            {
                return;
            }

            _opened = !_opened;
            UpdateNote(_opened);
        }

        private void UpdateNote(bool opened)
        {
            _noteGameObject.SetActive(_opened);
        }
    }
}