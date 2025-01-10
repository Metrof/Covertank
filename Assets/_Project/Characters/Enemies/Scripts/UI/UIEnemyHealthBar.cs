using UnityEngine;
using UnityEngine.UI;

public class UIEnemyHealthBar : PooledItem
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private Vector3 _offsetPos = new Vector3(0, 1, 0);

    private Transform _cameraTransform;

    public void Init(Transform transform)
    {
        _cameraTransform = transform;
    }
    public void SetFillAmount(float amount)
    {
        _fillImage.fillAmount = amount;
    }
    public void UpdatePositionAndRotation(Vector3 enemyPos)
    {
        transform.position = enemyPos + _offsetPos;
        transform.LookAt(transform.position + _cameraTransform.forward);
    }
    public void DestroyBar()
    {
        ReturnToPool();
    }
}
