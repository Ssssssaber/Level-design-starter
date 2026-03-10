using Health;
using UnityEngine;

namespace Collectable
{
    public class HealthPotion : MonoBehaviour, ICollectable
    {
        public int HealAmount = 1;
        public bool CanCollect(GameObject collector)
        {
            if (collector.TryGetComponent(out HealthComponent health) && health.CurrentHealth < health.MaxHealth)
            {
                return true;
            }
            return false;
        }

        public void Collect(GameObject collector)
        {
            if (collector.TryGetComponent(out HealthComponent health) && health.CurrentHealth < health.MaxHealth)
            {
                health.Heal(HealAmount);
                Destroy(gameObject);
            }
        }
    }
}