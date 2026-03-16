using Unity.VisualScripting;
using UnityEngine;


namespace Interactable
{
    public class Chest : MonoBehaviour, IInteractable
    {
        public bool IsOpened { get; private set; }
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private SpriteRenderer _spriteRef;
        [SerializeField] private Sprite _closedSprite;
        [SerializeField] private Sprite _openedSprite;

        public bool CanInteract()
        {
            return !IsOpened;
        }

        public void Interact()
        {
            if (!CanInteract()) return;
            OpenChest();
        }

        private void OpenChest()
        {
            SetOpened(true);

            if (!_itemPrefab)
            {
                Debug.Log("Empty chest!");
                return;
            }

            GameObject droppedItem = Instantiate(_itemPrefab, transform.position + Vector3.down, Quaternion.identity);
        }

        public void SetOpened(bool opened)
        {
            IsOpened = opened;
            if (IsOpened)
            {
               _spriteRef.sprite = _openedSprite;
               return;
            }
            _spriteRef.sprite = _closedSprite;
        }
    }
}