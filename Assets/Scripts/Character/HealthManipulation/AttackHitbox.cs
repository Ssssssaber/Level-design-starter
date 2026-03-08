
using UnityEngine;

namespace Health
{
    public class AttackHitbox : MonoBehaviour
    {
        [SerializeField] private uint _damage = 1;
        [SerializeField] private DamageDealer _hitboxCollider;

        private void Awake()
        {
            _hitboxCollider.enabled = false;
            _hitboxCollider.SetDamage(_damage);
        }

        // Called by Animation Event
        public void EnableHitbox()
        {
            _hitboxCollider.SetEnabled(true);
        }

        // Called by Animation Event
        public void DisableHitbox()
        {
            _hitboxCollider.SetEnabled(false);
        }
    }
}
