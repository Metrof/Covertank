using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class TankZoneTrigger : MonoBehaviour
{
    public UnityAction OnTankEnter;
    public UnityAction OnTankLeave;

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
        if (other.CompareTag("Tank"))
        {
            OnTankEnter?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tank"))
        {
            OnTankLeave?.Invoke();
        }
    }
}
