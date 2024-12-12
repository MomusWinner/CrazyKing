using UnityEngine;

namespace Health
{
    public interface IDamageable
    {
        int Health { get; }
        bool Damage(int damage);
        void Heal(int health);
        void OnDead();
    }
}