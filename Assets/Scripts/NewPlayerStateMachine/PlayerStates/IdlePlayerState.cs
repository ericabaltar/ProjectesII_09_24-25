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
        stateMachine.InputReader.RotateLeftEvent += stateMachine.RotateLeft;
        stateMachine.InputReader.RotateRightEvent += stateMachine.RotateRight;
        
        stateMachine.particlesRunning.Stop();
        //Debug.Log("Idle State");
    }



    public override void Tick(float deltaTime)
    {
        stateMachine.CheckGroundPlayer();
        stateMachine.StretchAndSquash();
        if (stateMachine.InputReader.MovementValue.x> 0f || stateMachine.InputReader.MovementValue.x < 0f)
        {
            stateMachine.SwitchState(new MovingPlayerState(stateMachine));
            return;
        }

        if (stateMachine.isWallWalking)
        {
            stateMachine.SwitchState(new WallWalkingState(stateMachine));
            return;
        }

    }


    public override void FixedTick()
    {
        
    }

    public override void Exit()
    {
        stateMachine.InputReader.RotateLeftEvent -= stateMachine.RotateLeft;
        stateMachine.InputReader.RotateRightEvent -= stateMachine.RotateRight;
        
    }


}
