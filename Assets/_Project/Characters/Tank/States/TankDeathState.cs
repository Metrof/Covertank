
public class TankDeathState : State
{
    private Tank _tank;

    public TankDeathState(Tank tank, BaseStateMachine stateMachine) : base(stateMachine)
    {
        _tank = tank;
    }
    public override void Enter()
    {
        base.Enter();
        _tank.NearestsEnemy.Clear();
        _tank.EnemyTrigger.gameObject.SetActive(false);
        _tank.PlayerTrigger.gameObject.SetActive(false);
        _tank.PeriodicDamage.Stop();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
