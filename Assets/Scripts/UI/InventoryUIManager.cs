using System.Collections.Generic;
using Interactable;
using Inventory;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private int _maxHeartsPerRow = 5; // Max hearts per row before wrapping


    private InventoryManager _playerInventory;
    private List<GameObject> _inventoryObjects = new List<GameObject>();
    private GridLayoutGroup _gridLayout;

    private void Awake()
    {
        _gridLayout = GetComponent<GridLayoutGroup>();
    }

    private void Start()
    {
        _playerInventory = GameManager.Instance.Player.GetComponent<InventoryManager>();
        
        //_playerInventory.OnInteractableAdded.AddListener(UpdateInventory);
    }

    //private void UpdateInventory(IInteractable interactable)
    //{
    //    foreach (var item in _inventoryObjects)
    //    {
    //        Destroy(item);
    //    }
    //    _inventoryObjects.Clear();

    //    for (int i = 0; i < currentHearts; i++)
    //    {
    //        GameObject heart = Instantiate(_heartPrefab, transform);
    //        _inventoryObjects.Add(heart);
    //    }

    //    _gridLayout.constraintCount = _maxHeartsPerRow;
    //}
}
