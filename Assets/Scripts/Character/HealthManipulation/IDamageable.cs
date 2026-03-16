namespace Health
{
    public interface IDamageable
    {
        bool TakeDamage(int amount);
        bool Heal(int amount);
    }
}