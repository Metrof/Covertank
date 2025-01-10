using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : PooledItem
{
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _lifeTime = 2;

    private Rigidbody rb;
    private float _damage;
    private Vector3 _flyDirection;

    private bool _isFly;
    private float _startFlyTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ShotingFromWeapon(float damage, Vector3 flyDirection)
    {
        _flyDirection = flyDirection;
        _damage = damage;
        _isFly = true;
        _startFlyTime = Time.time;
    }
    private void FlyEnd()
    {
        _isFly = false;
        ReturnToPool();
    }
    private void FixedUpdate()
    {
        if (_isFly)
        {
            if (Time.time - _startFlyTime > _lifeTime)
            {
                FlyEnd();
                return;
            }
            rb.MovePosition(transform.position + _flyDirection * Time.fixedDeltaTime * _bulletSpeed);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageTaker>(out var taker))
        {
            if (taker.IsEnable)
            {
                taker.TakeDamage(_damage);
            }
        }
        FlyEnd();
    }
}
