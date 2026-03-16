using System;
using UnityEngine;
using UnityEngine.Events;

namespace Health
{
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _maxHealth = 3;
        [SerializeField] private int _currentHealth;

        public UnityEvent<int> OnHealthChanged; // currentHealth
        public UnityEvent OnDeath;
        public UnityEvent OnTakeDamage;

        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;
        public bool IsAlive => _currentHealth > 0;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public bool TakeDamage(int damage)
        {
            if (!IsAlive) return false;

            if (_currentHealth - damage <= 0)
            {
                Die();
            }

            _currentHealth = _currentHealth - (int)damage;
            OnHealthChanged?.Invoke(_currentHealth);
            OnTakeDamage?.Invoke();

            return true;
        }

        public bool Heal(int amount)
        {
            bool maxedAlready = _currentHealth  >= _maxHealth;
            
            if (maxedAlready)
            {
                return maxedAlready;
            }

            _currentHealth = _currentHealth + (int)amount > _maxHealth ? _maxHealth : _currentHealth + (int)amount;
            OnHealthChanged?.Invoke(_currentHealth);
            return true;
        }

        public void Die()
        {
            if (!IsAlive) return;

            _currentHealth = 0;
            OnDeath?.Invoke();
            Debug.Log($"{gameObject.name} died!");
        }
    }
}