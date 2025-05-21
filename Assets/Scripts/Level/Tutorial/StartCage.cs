using Entity.Servant;
using Health;
using UnityEngine;
using VContainer;

public class StartCage : MonoBehaviour, IDamageable
{
    public int Health => 1;
    public int MaxHealth => 1;

    [Inject] private ServantManager _servantManager;
    [SerializeField] private string _particlePath;
    [SerializeField] private ServantType _servantType;
    private ServantController _servant;
    private bool _servantIsCaptured;
    
    public void Update()
    {
        if (_servantManager._isInitialized && !_servantIsCaptured)
        {
            _servantIsCaptured = true;
            ServantController servant = _servantManager.CreateCaptiveServant(_servantType, transform.position);
        }
    }

    public void ChangeMaxHealth(int maxHealth)
    {
    }

    public bool Damage(int damage)
    {
        GameObject particleObj = Resources.Load<GameObject>(_particlePath);
        Instantiate(particleObj, transform.position, Quaternion.identity);
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