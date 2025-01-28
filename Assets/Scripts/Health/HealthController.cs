using System;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Health
{
    public class HealthController : MonoBehaviour
    {
        public Action OnDeath;
        public int Health => _health;
        public int MaxHealth => _maxHealth;

        [SerializeField] private Slider _healthBar;
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private bool _hideOnCalm;
        [ShowIf(nameof(_hideOnCalm))]
        [SerializeField] private float _disableHpBarTime;
        
        private int _maxHealth;
        private int _health;
        private bool _isDead;
        private float _elapsedTime;

        public void Start()
        {
            if (_hideOnCalm)
                HideBar();
        }
        
        public void Setup(int maxHealth, Action onDeath)
        {
            _maxHealth = maxHealth;
            OnDeath = onDeath;
            _health = maxHealth;
            _healthBar.maxValue = _maxHealth;
            _healthBar.value = _maxHealth;
        }

        public void Update()
        {
            if (!_hideOnCalm) return;
            
            if (_elapsedTime >= _disableHpBarTime)
            {
                HideBar();
                _elapsedTime = 0;
            }
            else
                _elapsedTime += Time.deltaTime;
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
                OnDeath?.Invoke();
                _isDead = true;
                return true;
            }
            
            ShowBar();
            
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
            SetHealth(_maxHealth);
        }

        private void HideBar()
        {
            _healthBar.gameObject.SetActive(false);
        }

        private void ShowBar()
        {
            _elapsedTime = 0;
            _healthBar.gameObject.SetActive(true);
        }
    }
}