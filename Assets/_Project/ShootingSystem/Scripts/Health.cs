using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageTaker
{
    public bool IsEnable { get; set; }
    public Transform Transform { get { return transform; } }


    public UnityAction OnDead;

    public int MaxHealth = 10;

    protected float _currentHealth;
    public virtual float CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            _currentHealth = Mathf.Clamp(value, 0 , MaxHealth);
        }
    }

    protected EventBus _eventBus;

    public void Initialize(EventBus eventBus)
    {
        _eventBus = eventBus;
    }
    public void RestoreFullHP()
    {
        CurrentHealth = MaxHealth;
        IsEnable = true;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
    }
}
