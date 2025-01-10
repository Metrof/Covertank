using System;
using System.Collections.Generic;

public class BaseStateMachine
{
    public State CurrentState;
    private Dictionary<Type, State> _states = new Dictionary<Type, State>();

    public void AddState(State state)
    {
        _states.Add(state.GetType(), state);
    }
    public void ChangeState(Type newState)
    {
        CurrentState?.Exit();
        CurrentState = _states[newState];
        CurrentState.Enter();
    }
}
