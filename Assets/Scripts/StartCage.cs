using System.Linq;
using Health;
using Servant;
using UnityEngine;
using VContainer;

public class StartCage : MonoBehaviour, IDamageable
{
    public int Health => 1;
    public int MaxHealth => 1;

    [Inject] private ServantManager _servantManager;
    private ServantController _servant;
    private bool _servantIsCaptured;
    
    public void Update()
    {
        if (_servantManager._isInitialized && !_servantIsCaptured)
        {
            _servant = _servantManager.Servants.First();
            _servant.transform.position = transform.position;
            _servantIsCaptured = true;
        }
    }

    public void ChangeMaxHealth(int maxHealth)
    {
        
    }

    public bool Damage(int damage)
    {
        Destroy(gameObject);
        return true;
    }

    public void Heal(int health)
    {
    }
    
    public void OnDead()
    {
    }
}