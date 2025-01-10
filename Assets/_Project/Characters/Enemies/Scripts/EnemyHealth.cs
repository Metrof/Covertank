using UnityEngine.Events;

public class EnemyHealth : Health
{
    public UnityAction<float> OnHPChange;
    public override float CurrentHealth
    {
        get => base.CurrentHealth;
        set
        {
            base.CurrentHealth = value;
            OnHPChange?.Invoke(CurrentHealth);
            if (_currentHealth == 0)
            {
                IsEnable = false;
                OnDead?.Invoke();
            }
        }
    }
}
