using UnityEngine;
using Interactable;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private List<IInteractable> _items = new List<IInteractable>();
        [SerializeField] private int _maxCapacity = 10;
        [SerializeField] private int _currentCapacity = 0;

        public UnityEvent<IInteractable> OnInteractableAdded;

        public bool AddInteractable(IInteractable interactable)
        {
            _currentCapacity = _items.Count;
            if (_currentCapacity >= _maxCapacity)
            {
                return false;
            }

            Debug.LogWarning($"Interactable added: {interactable.GetType().Name}");
            _items.Add(interactable);
            OnInteractableAdded?.Invoke(interactable);
            
            _currentCapacity = _items.Count;
            return true;
        }

       public IInteractable FindFirstItemWithTag(string tag)
        {
            foreach (var item in _items)
            {
                if (item.GetTag() == tag)
                {
                    return item;
                }
            }
            return null; // No matching item found
        }

        public bool TryUseFirstItemWithTag(string tag, out IInteractable item)
    {
        item = FindFirstItemWithTag(tag);
        if (item != null)
        {
            _items.Remove(item);
            _currentCapacity = _items.Count;
            return true;
        }
        return false; // No matching item found
    }


    }
}