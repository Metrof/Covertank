using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour 
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private int _poolSize = 20;

    private Pool<Bullet> _defaultBulletPool;

    public void CreatePools()
    {
        List<Bullet> list = new List<Bullet>();
        for (int i = 0; i < _poolSize; i++)
        {
            var bullet = Instantiate(_bulletPrefab, transform);
            bullet.transform.position = transform.position;
            list.Add(bullet);
        }
        _defaultBulletPool = new Pool<Bullet>(list);
    }
    public bool TryGetDefaultBullet(out Bullet instantiateEntity, Vector3 position, Quaternion rotation)
    {
        return _defaultBulletPool.TryInstantiate(out instantiateEntity, position, rotation);
    }
}
