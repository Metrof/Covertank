using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class PeriodicDamage
{
    public UnityAction<float> OnDamageTake;

    private float _timeBetweenDamage = 0.1f;
    private float _damagePerTick = 1f;

    private CancellationTokenSource _cancellationTokenSource;

    public PeriodicDamage(float timeBetweenDamage, float damagePerTick)
    {
        _timeBetweenDamage = timeBetweenDamage;
        _damagePerTick = damagePerTick;
    }
    public void ChangeState(bool isPlayerEnter)
    {
        if (isPlayerEnter)
        {
            StopOp();
        }
        else
        {
            _cancellationTokenSource = new CancellationTokenSource();
            StartGettingDamageAsync(_cancellationTokenSource.Token).Forget();
        }
    }
    public void Stop()
    {
        StopOp();
    }
    private async UniTaskVoid StartGettingDamageAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            OnDamageTake?.Invoke(_damagePerTick);
            await UniTask.WaitForSeconds(_timeBetweenDamage);
        }
    }
    private void StopOp()
    {
        if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
    }
}
