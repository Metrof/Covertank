using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerCalmStates : State
{
    private Player _player;
    private CancellationTokenSource _cancellationTokenSource;
    public PlayerCalmStates(Player player ,BaseStateMachine stateMachine) : base(stateMachine)
    {
        _player = player;
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
