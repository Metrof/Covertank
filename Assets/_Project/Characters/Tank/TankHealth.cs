
public class TankHealth : Health
{
    public override float CurrentHealth 
    { 
        get => base.CurrentHealth;
        set
        {
            base.CurrentHealth = value;
            _eventBus.RaiseEvent<ICanvas>(c => c.TankHPChage(CurrentHealth, MaxHealth));
            if (_currentHealth == 0)
            {
                IsEnable = false;
                _eventBus.RaiseEvent<IGameManager>(g => g.GameLost());
                OnDead?.Invoke();
            }
        }
    }
}
