using UnityEngine;
using UnityEngine.Events;

public class SpawnLvlTrigger : MonoBehaviour
{
    public UnityAction OnTankNear;

    private void OnTriggerEnter(Collider other)
    {
        OnTankNear?.Invoke();
        gameObject.SetActive(false);
    }
}
