using UnityEngine;

public class TankOnBaseState : State
{
    private Tank _tank;

    public TankOnBaseState(Tank tank, BaseStateMachine stateMachine) : base(stateMachine)
    {
        _tank = tank;
    }
    public override void Enter()
    {
        base.Enter();
        _tank.NearestsEnemy.Clear();
        _tank.Health.RestoreFullHP();
        _tank.EnemyTrigger.gameObject.SetActive(false);
        _tank.PlayerTrigger.gameObject.SetActive(false);
        _tank.PeriodicDamage.Stop();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
