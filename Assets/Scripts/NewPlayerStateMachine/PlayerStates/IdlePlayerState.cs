using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePlayerState : PlayerBaseState
{
    public IdlePlayerState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entering Idle");
    }


    public override void Tick(float deltaTime)
    {
        Debug.Log("Ticking Idle");
        if(Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.SwitchState(new MovingPlayerState(stateMachine));
        }
    }


    public override void Exit()
    {
        Debug.Log("Leaving Idle");
    }
}
