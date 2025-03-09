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
        stateMachine.InputReader.RotateLeftEvent += stateMachine.RotateLeft;
        stateMachine.InputReader.RotateRightEvent += stateMachine.RotateRight;

    }



    public override void Tick(float deltaTime)
    {
        stateMachine.CheckGroundPlayer();

        

        if(stateMachine.isGrounded)
        {
            stateMachine.particlesRunning.Play();
        }
        else
        {
            stateMachine.particlesRunning.Stop();
        }

        //Debug.Log("MOVING");
        stateMachine.AnimateAndRotateAccording();
        stateMachine.StretchAndSquash();

        if (Mathf.Abs(stateMachine.InputReader.MovementValue.x) < 0.01f && Mathf.Abs(stateMachine.rigidbody2d.velocity.x) < 0.01f)
        {
            stateMachine.SwitchState(new IdlePlayerState(stateMachine));
        }

        if (stateMachine.isWallWalking)
        {
            stateMachine.SwitchState(new WallWalkingState(stateMachine));
        }

    }


    public override void FixedTick()
    {
        
        if (stateMachine.InputReader.MovementValue.x != 0)
            stateMachine.rigidbody2d.velocity = new Vector2(stateMachine.InputReader.MovementValue.x * 12, stateMachine.rigidbody2d.velocity.y);
        else
            stateMachine.rigidbody2d.velocity = new Vector2(0.0f, stateMachine.rigidbody2d.velocity.y);
    }

    public override void Exit()
    {
        stateMachine.InputReader.RotateLeftEvent -= stateMachine.RotateLeft;
        stateMachine.InputReader.RotateRightEvent -= stateMachine.RotateRight;
    }
}
