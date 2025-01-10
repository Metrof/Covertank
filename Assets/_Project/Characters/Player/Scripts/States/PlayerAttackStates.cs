using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class PlayerAttackStates : State
{
    private Player _player;
    private CancellationTokenSource _cancellationTokenSource;
    private float _shotStartDelay = 0.2f;
    public PlayerAttackStates(Player player, BaseStateMachine stateMachine) : base(stateMachine)
    {
        _player = player;
    }
    public override void Enter()
    {
        base.Enter();
        _player.IsAttack = true;
        StartPlayerShot();
    }
    private void StartPlayerShot()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        StartShotAsync(_cancellationTokenSource.Token).Forget();
    }
    private async UniTaskVoid StartShotAsync(CancellationToken token)
    {
        _player.NearestEnemyPosition = _player.NearestsEnemy[0].Transform;
        await UniTask.WaitForSeconds(_shotStartDelay);
        while (!token.IsCancellationRequested)
        {
            if (_player.NearestsEnemy.Count == 0)
            {
                return;
            }
            IDamageTaker nearestEnemy = _player.NearestsEnemy[0];
            foreach (var enemy in _player.NearestsEnemy)
            {
                if (Vector3.Distance(_player.transform.position, enemy.Transform.position) < Vector3.Distance(_player.transform.position, nearestEnemy.Transform.position))
                {
                    nearestEnemy = enemy;
                }
            }
            _player.NearestEnemyPosition = nearestEnemy.Transform;
            if (nearestEnemy.IsEnable)
            {
                _player.Weapon.Shoot();
            }
            else
            {
                _player.RemoveEnemyFromList(nearestEnemy);
            }
            await UniTask.WaitForSeconds(_player.Weapon.ShootRate);
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
        _player.IsAttack = false;
        StopShoot();
    }
}
