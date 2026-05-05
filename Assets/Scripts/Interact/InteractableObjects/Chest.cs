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

        public bool CanInteract(GameObject interactor)
        {
            return !IsOpened;
        }

        public void Interact(GameObject interactor)
        {
            if (!CanInteract(interactor)) return;
            OpenChest(interactor);
        }

        private void OpenChest(GameObject interactor)
        {
            SetOpened(true);

            if (!_itemPrefab)
            {
                Debug.Log("Empty chest!");
                return;
            }

            GameObject droppedItem = Instantiate(_itemPrefab, transform.position + Vector3.down, Quaternion.identity);
            GameManager.Instance.MoveObjectToEnvironment(droppedItem);
            // Play pickup sound for chest loot pickup
            var profile = interactor?.GetComponent<GameObjectsSound.SoundProfileContainer>()?.GetProfile();
            if (profile != null)
            {
                GameManager.Instance.FXSoundPlayer.PlaySound(GameObjectsSound.SoundID.Pickup, profile, transform);
            }
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

        public string GetTag()
        {
            return "Chest";
        }
    }
}
