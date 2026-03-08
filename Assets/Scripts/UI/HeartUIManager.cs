using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Health;

public class HeartUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _heartPrefab; // Assign in Inspector
    [SerializeField] private int _maxHeartsPerRow = 5; // Max hearts per row before wrapping

    [SerializeField] private HealthComponent _playerHealth;
    private List<GameObject> _heartObjects = new List<GameObject>();
    private GridLayoutGroup _gridLayout;

    private void Awake()
    {
        _gridLayout = GetComponent<GridLayoutGroup>();
        _playerHealth.OnHealthChanged.AddListener(UpdateHearts);
    }

    private void Start()
    {
        UpdateHearts(_playerHealth.CurrentHealth);
    }

    private void UpdateHearts(int currentHearts)
    {
        foreach (var heart in _heartObjects)
        {
            Destroy(heart);
        }
        _heartObjects.Clear();

        for (int i = 0; i < currentHearts; i++)
        {
            GameObject heart = Instantiate(_heartPrefab, transform);
            _heartObjects.Add(heart);
        }

        _gridLayout.constraintCount = _maxHeartsPerRow;
    }
}
