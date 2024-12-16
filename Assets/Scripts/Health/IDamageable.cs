using UnityEngine;

namespace Health
{
    public interface IDamageable
    {
        int Health { get; }
        int MaxHealth { get; }
        void ChangeMaxHealth(int maxHealth);
        bool Damage(int damage);
        void Heal(int health);
        void OnDead();
    }
}