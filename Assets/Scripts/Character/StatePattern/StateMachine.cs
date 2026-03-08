using Health;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [HideInInspector] public HealthComponent Health { get; private set; }

    private void Awake()
    {
        // Try to get the HealthComponent (optional)
        Health = GetComponent<HealthComponent>();

        if (Health != null)
        {
            Health.OnDeath.AddListener(OnDeath);
            Health.OnTakeDamage.AddListener(OnTakeDamage);
        }

        Initialize();
    }

    protected virtual void Initialize() { } 

    public virtual void OnDeath()
    {
        // Override in derived classes (e.g., PlayerStateMachine, NPCStateMachine)
    }

    public virtual void OnTakeDamage()
    {
        // Override in derived classes
    }
}
