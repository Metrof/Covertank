using Cysharp.Threading.Tasks;

public class EnemyDeadState : State
{
    private Enemy _enemy;
    private float _fallingDuration = 4;
    public EnemyDeadState(Enemy enemy, BaseStateMachine stateMachine) : base(stateMachine)
    {
        _enemy = enemy;
    }
    public override void Enter()
    {
        base.Enter();
        _enemy.TankTrigger.gameObject.SetActive(false);
        _enemy.Collider.enabled = false;
        _enemy.HealthBar.DestroyBar();
        Falling().Forget();
    }
    private async UniTaskVoid Falling()
    {
        _enemy.Rigidbody.isKinematic = false;
        await UniTask.WaitForSeconds(_fallingDuration);
        _enemy.Rigidbody.isKinematic = true;
    }
    public override void Exit()
    {
        base.Exit();
        _enemy.Collider.enabled = true;
        _enemy.TankTrigger.gameObject.SetActive(true);
    }
}
