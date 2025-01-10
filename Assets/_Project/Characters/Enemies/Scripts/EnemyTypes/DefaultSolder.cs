using UnityEngine;

public class DefaultSolder : Enemy
{
    public override void Initialization(Context context, IDamageTaker target)
    {
        base.Initialization(context, target);

        _baseStateMachine = new BaseStateMachine();
        _baseStateMachine.AddState(new EnemyIdleState(this, _baseStateMachine));
        _baseStateMachine.AddState(new EnemyAttackState(this, _baseStateMachine));
        _baseStateMachine.AddState(new EnemyMoveState(this, _baseStateMachine));
        _baseStateMachine.AddState(new EnemyDeadState(this, _baseStateMachine));
        _baseStateMachine.ChangeState(typeof(EnemyIdleState));
    }
    public override void PlaceOnDefaultPosition()
    {
        base.PlaceOnDefaultPosition();
        CalmState();
    }
    private void Update()
    {
        if (_baseStateMachine != null)
        {
            _baseStateMachine.CurrentState.StateUpdate();
        }
    }
    public override void CalmState()
    {
        base.CalmState();
        _baseStateMachine.ChangeState(typeof(EnemyIdleState));
    }
    public override void SearchState()
    {
        base.SearchState();
        _baseStateMachine.ChangeState(typeof(EnemyMoveState));
    }
}
