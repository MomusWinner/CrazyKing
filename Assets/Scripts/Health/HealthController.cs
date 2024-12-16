using System;
using TMPro;
using UnityEngine.UI;

namespace Health
{
    public class HealthController
    {
        public int Health => _health;
        public int MaxHealth => _maxHealth;
        
        private readonly Slider _healthBar;
        private readonly TMP_Text _healthText;
        private readonly Action _onDeath;
        private int _maxHealth;
        private int _health;
        private bool _isDead;
        
        public HealthController(int maxHealth, HealthUIData healthUIData, Action onDeath)
        {
            _maxHealth = maxHealth;
            _healthBar = healthUIData.slider;
            _healthText = healthUIData.healthText;
            _onDeath = onDeath;
            _health = maxHealth;
            _healthBar.maxValue = _maxHealth;
            _healthBar.value = _maxHealth;
        }

        private void SetHealth(int value)
        {
            _health = value;
            _healthBar.value = value;
            _healthText.text = $"{_health} / {_maxHealth}";
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

        public void ChangeMaxHealth(int maxHealth)
        {
            _maxHealth = maxHealth;
            _healthBar.maxValue = maxHealth;
            _healthText.text = $"{_health} / {_maxHealth}";
        }
    }
}