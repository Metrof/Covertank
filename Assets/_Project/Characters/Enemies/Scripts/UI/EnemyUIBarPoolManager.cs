using System.Collections.Generic;
using UnityEngine;

public class EnemyUIBarPoolManager : MonoBehaviour
{
    [SerializeField] private UIEnemyHealthBar _bulletPrefab;
    [SerializeField] private int _poolSize = 20;
    [SerializeField] private Camera _camera;

    private Pool<UIEnemyHealthBar> _pool;

    public void CreatePools()
    {
        List<UIEnemyHealthBar> list = new List<UIEnemyHealthBar>();
        for (int i = 0; i < _poolSize; i++)
        {
            var obj = Instantiate(_bulletPrefab, transform);
            obj.transform.position = transform.position;
            obj.Init(_camera.transform);
            list.Add(obj);
        }
        _pool = new Pool<UIEnemyHealthBar>(list);
    }
    public bool TryGetPoolItem(out UIEnemyHealthBar instantiateEntity, Vector3 position, Quaternion rotation)
    {
        return _pool.TryInstantiate(out instantiateEntity, position, rotation);
    }
}
