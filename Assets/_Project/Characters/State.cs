
public class State 
{
    protected BaseStateMachine _stateMachine;

    public State(BaseStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
    public virtual void Enter()
    {

    }
    public virtual void StateUpdate()
    {

    }
    public virtual void Exit()
    {

    }
}
