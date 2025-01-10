using System;
using UnityEngine;

public abstract class PooledItem : MonoBehaviour
{
    public event Action<PooledItem> OnDestroy;

    protected void ReturnToPool()
    {
        gameObject.SetActive(false);
        OnDestroy?.Invoke(this);
    }
}
