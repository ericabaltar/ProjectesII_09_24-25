using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlayerState : PlayerBaseState
{
    public MovingPlayerState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entering Moving");
    }



    public override void Tick(float deltaTime)
    {
        Debug.Log("Ticking Moving");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.SwitchState(new IdlePlayerState(stateMachine));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Moving");
    }
}
