using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class PlayerZoneTrigger : MonoBehaviour
{
    public UnityAction OnPlayerEnter;
    public UnityAction OnPlayerLeave;

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
        if (other.CompareTag("Player"))
        {
            OnPlayerEnter?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerLeave?.Invoke();
        }
    }
}
