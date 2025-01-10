
using UnityEngine;

public interface IDamageTaker 
{
    Transform Transform { get; }
    bool IsEnable { get; }
    public void TakeDamage(float damage);
}
