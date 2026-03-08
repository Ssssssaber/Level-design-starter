using Unity.VisualScripting;
using UnityEngine;

namespace Health
{
    [RequireComponent(typeof(Collider2D))]
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField] private uint _damageAmount = 1;
        private Collider2D _attachedCollider;

        private void Awake()
        {
            _attachedCollider = GetComponent<Collider2D>();
            _attachedCollider.isTrigger = true;
        }

        public void SetDamage(uint damage)
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

