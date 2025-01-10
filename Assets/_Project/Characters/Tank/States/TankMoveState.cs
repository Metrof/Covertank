using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class TankMoveState : State
{
    private Tank _tank;
    private CancellationTokenSource _cancellationTokenSource;
    private int _nextPoint;

    public TankMoveState(Tank tank, BaseStateMachine stateMachine) : base(stateMachine)
    {
        _tank = tank;
        _tank.OnTankDestroy += CanselMove;
    }
    public override void Enter()
    {
        base.Enter();
        StartAgentMove();
        _tank.MeshAgent.isStopped = false;
        _tank.EnemyTrigger.gameObject.SetActive(true);
        _tank.PlayerTrigger.gameObject.SetActive(true);
    }
    private void StartAgentMove()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        StartMoveAsync(_cancellationTokenSource.Token).Forget();
    }
    private async UniTaskVoid StartMoveAsync(CancellationToken token)
    {
        for (int i = _tank.NextMovePoint; i < _tank.Path.Count; i++)
        {
            _nextPoint = i;
            _tank.MeshAgent.SetDestination(_tank.Path[_nextPoint]);
            while (!token.IsCancellationRequested && Vector3.Distance(_tank.MeshAgent.transform.position, _tank.Path[_nextPoint]) > _tank.MeshAgent.radius * 2.5f)
            {
                await UniTask.Yield();
            }
            if (token.IsCancellationRequested)
            {
                return;
            }
        }
        _tank.EventBus.RaiseEvent<IGameManager>(g => g.LvlEnd());
        _stateMachine.ChangeState(typeof(TankOnBaseState));
    }
    private void CanselMove()
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
        _tank.NextMovePoint = _nextPoint;
        _tank.MeshAgent.isStopped = true;
        CanselMove();
    }
}
