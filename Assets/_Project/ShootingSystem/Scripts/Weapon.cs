using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private float _damage = 1;
    [SerializeField] private int _layer;

    public float ShootRate = 1;

    private BulletPoolManager _poolManager;

    public void Init(BulletPoolManager poolManager)
    {
        _poolManager = poolManager;
    }
    public void Shoot()
    {
        if (_poolManager.TryGetDefaultBullet(out var bullet, _shotPoint.position, Quaternion.identity))
        {
            bullet.gameObject.layer = _layer;
            bullet.ShotingFromWeapon(_damage, transform.forward);
        }
    }
}
