using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class RotatingPlayerState : PlayerBaseState
{
    public RotatingPlayerState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Rotating State");

    }

    public override void Tick(float deltaTime)
    {
        if (!GameManager.Instance.isRotating && stateMachine.isWallWalking)
        {
            stateMachine.SwitchState(new WallWalkingState(stateMachine));

            return;
        }
        else if (stateMachine.isWallWalking == true) {
            stateMachine.SwitchState(new WallWalkingState(stateMachine));
            return;
        }
        else
            stateMachine.SwitchState(new IdlePlayerState(stateMachine));
    }


    public override void FixedTick()
    {

    }

    public override void Exit()
    {

    }
}
