using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class LvlWaveTrigger : MonoBehaviour
{
    public UnityAction OnTankEnter;

    private void OnTriggerEnter(Collider other)
    {
        OnTankEnter?.Invoke();
    }
}
