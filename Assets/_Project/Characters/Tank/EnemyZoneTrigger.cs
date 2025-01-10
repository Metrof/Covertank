using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class EnemyZoneTrigger : MonoBehaviour
{
    public UnityAction<IDamageTaker> OnEnemyEnter;
    public UnityAction<IDamageTaker> OnEnemyLeave;

    private SphereCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }
    public void Init(float radius)
    {
        _collider.radius = radius;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.TryGetComponent<IDamageTaker>(out var taker) && taker.IsEnable)
            {
                OnEnemyEnter?.Invoke(taker);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.TryGetComponent<IDamageTaker>(out var taker) && taker.IsEnable)
            {
                OnEnemyLeave?.Invoke(taker);
            }
        }
    }
}
