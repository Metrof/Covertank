using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class EnemyAttackState : State
{
    private Enemy _enemy;
    private CancellationTokenSource _cancellationTokenSource;

    public EnemyAttackState(Enemy enemy, BaseStateMachine stateMachine) : base(stateMachine)
    {
        _enemy = enemy;
    }
    public override void Enter()
    {
        base.Enter();
        StartShoot();
    }
    private void StartShoot()
    {
        if (_cancellationTokenSource == null)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            ShootAsync(_cancellationTokenSource.Token).Forget();
        }
    }
    private async UniTaskVoid ShootAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (_enemy.Target.IsEnable)
            {
                _enemy.transform.rotation = Quaternion.LookRotation(_enemy.Target.Transform.position - _enemy.transform.position);
                _enemy.Weapon.Shoot();
                await UniTask.WaitForSeconds(_enemy.Weapon.ShootRate);
            }
            else
            {
                StopShoot();
            }
        }
    }
    private void StopShoot()
    {
        if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
        }
    }
    public override void StateUpdate()
    {
        base.StateUpdate();
        _enemy.HealthBar.UpdatePositionAndRotation(_enemy.transform.position);
    }
    public override void Exit()
    {
        base.Exit();
        StopShoot();
    }
}
