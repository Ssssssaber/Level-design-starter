using Unity.VisualScripting;
using UnityEngine;

namespace Health
{
    [RequireComponent(typeof(Collider2D))]
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField] private int _damageAmount = 1;
        [SerializeField] private bool _enabledFromStart = true;
        private Collider2D _attachedCollider;

        private void Awake()
        {
            _attachedCollider = GetComponent<Collider2D>();
            _attachedCollider.isTrigger = true;
            SetEnabled(_enabledFromStart);
        }

        public void SetDamage(int damage)
        {
            _damageAmount = damage;
        }

        public void SetEnabled(bool enabled)
        {
            _attachedCollider.enabled = enabled;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damageAmount);
            }
        }
    }
}

