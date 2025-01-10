using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : PooledItem
{
    private readonly Queue<T> _available;

    public int PoolSize { get { return _available.Count; } }

    public Pool(List<T> _itemList)
    {
        _available = new Queue<T>();
        for (int i = 0; i < _itemList.Count; i++)
        {
            var entity = _itemList[i];
            entity.gameObject.SetActive(false);
            entity.OnDestroy += item => _available.Enqueue(item as T);
            _available.Enqueue(entity);
        }
    }

    public bool TryInstantiate(out T instantiateEntity, Vector3 position, Quaternion rotation)
    {
        if (_available.Count > 0)
        {
            instantiateEntity = _available.Dequeue();
            instantiateEntity.transform.SetPositionAndRotation(position, rotation);
            instantiateEntity.gameObject.SetActive(true);
            return true;
        }

        instantiateEntity = null;
        return false;
    }
}
