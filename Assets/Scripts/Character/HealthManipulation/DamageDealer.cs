using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private uint _damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_damageAmount);
        }
    }
}
