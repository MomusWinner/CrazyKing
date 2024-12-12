using System;
using UnityEngine.UI;

namespace Health
{
    public class HealthController
    {
        public int Health => _health;
        
        private readonly int _maxHealth;
        private readonly Slider _healthBar;
        private readonly Action _onDeath;
        private int _health;
        private bool _isDead;
        
        public HealthController(int maxHealth, Slider healthBar, Action onDeath)
        {
            _maxHealth = maxHealth;
            _healthBar = healthBar;
            _onDeath = onDeath;
            _health = maxHealth;
            _healthBar.maxValue = _maxHealth;
            _healthBar.value = _maxHealth;
        }

        private void SetHealth(int value)
        {
            _health = value;
            _healthBar.value = value;
        }

        public bool Damage(int damage)
        {
            if (_isDead)
                return true;

            if (damage >= _health)
            {
                SetHealth(0);
                _onDeath?.Invoke();
                _isDead = true;
                return true;
            }
            
            SetHealth(_health - damage);
            return false;
        }

        public void Heal(int health)
        {
            if (_isDead)
                return;

            if (health + _health >= _maxHealth)
            {
                SetHealth(_maxHealth);
                return;
            }
            
            SetHealth(_health + health);
        }
    }
}