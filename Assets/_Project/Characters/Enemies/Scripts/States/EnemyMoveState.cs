using UnityEngine;

public class EnemyMoveState : State
{
    private Enemy _enemy;
    public EnemyMoveState(Enemy enemy, BaseStateMachine stateMachine) : base(stateMachine)
    {
        _enemy = enemy;
    }
    public override void Enter()
    {
        base.Enter();
        _enemy.PlaceOnMesh();
        MoveToTank();
        _enemy.MeshAgent.isStopped = false;
    }
    public override void StateUpdate()
    {
        base.StateUpdate();
        _enemy.HealthBar.UpdatePositionAndRotation(_enemy.transform.position);
    }
    private void MoveToTank()
    {
        _enemy.MeshAgent.SetDestination(_enemy.Target.Transform.position);
    }
    public override void Exit()
    {
        base.Exit();
        _enemy.MeshAgent.isStopped = true;
    }
}
