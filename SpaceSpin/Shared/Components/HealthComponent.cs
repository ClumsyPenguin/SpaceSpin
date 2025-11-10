using System;
using Nez;

namespace SpaceSpin.Shared.Components;

public class HealthComponent : Component
{
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    public bool IsDead => CurrentHealth <= 0;

    public event Action<HealthComponent> OnDeath;

    public HealthComponent(int maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth < 0)
            CurrentHealth = 0;

        if (IsDead)
            OnDeath?.Invoke(this);
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;

        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }
}
