using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IdlePlayerState : PlayerBaseState
{
    public IdlePlayerState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.InputReader.RotateLeftEvent += RotateLeft;
        stateMachine.InputReader.RotateRightEvent += RotateRight;
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



    private void RotateLeft()
    {
        
    }

    private void RotateRight()
    {
        
    }
}
