using Health;
using Unity.VisualScripting;
using UnityEngine;

public class NPCDrop : MonoBehaviour
{
    [SerializeField] private GameObject _dropItemPrefab;
    [SerializeField] private HealthComponent _health;

    private void Start()
    {
        if (_dropItemPrefab == null) return;

        _health.OnDeath.AddListener(DropItem);
    }

    private void DropItem()
    {
        Instantiate(_dropItemPrefab, transform.position, Quaternion.identity);
    }
}