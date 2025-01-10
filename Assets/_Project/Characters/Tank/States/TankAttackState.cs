using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class TankAttackState : State
{
    private Tank _tank;
    private CancellationTokenSource _cancellationTokenSource;

    public TankAttackState(Tank tank, BaseStateMachine stateMachine) : base(stateMachine)
    {
        _tank = tank;
    }
    public override void Enter()
    {
        base.Enter();
        _tank.EnemyTrigger.gameObject.SetActive(true);
        _tank.PlayerTrigger.gameObject.SetActive(true);
        StartAgentShot();
    }
    private void StartAgentShot()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        StartShotAsync(_cancellationTokenSource.Token).Forget();
    }
    private async UniTaskVoid StartShotAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (_tank.NearestsEnemy.Count == 0)
            {
                _tank.StartMove();
                return;
            }
            IDamageTaker nearestEnemy = _tank.NearestsEnemy[0];
            foreach (var enemy in _tank.NearestsEnemy)
            {
                if (Vector3.Distance(_tank.transform.position, enemy.Transform.position) < Vector3.Distance(_tank.transform.position, nearestEnemy.Transform.position))
                {
                    nearestEnemy = enemy;
                }
            }
            if (nearestEnemy.IsEnable)
            {
                _tank.Weapon.transform.rotation = Quaternion.LookRotation(nearestEnemy.Transform.position - _tank.transform.position);
                _tank.Weapon.Shoot();
            }
            else
            {
                _tank.RemoveEnemyFromList(nearestEnemy);
            }
            await UniTask.WaitForSeconds(_tank.Weapon.ShootRate);
        }
    }
    private void StopShoot()
    {
        if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
    }
    public override void Exit()
    {
        base.Exit();
        StopShoot();
    }
}
