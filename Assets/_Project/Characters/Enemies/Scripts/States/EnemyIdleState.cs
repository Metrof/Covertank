using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : State
{
    private Enemy _enemy;
    public EnemyIdleState(Enemy enemy ,BaseStateMachine stateMachine) : base(stateMachine)
    {
        _enemy = enemy;
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
