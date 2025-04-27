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

        stateMachine.AnimateAndRotateAccording();
        stateMachine.StretchAndSquash();

        if (Mathf.Abs(stateMachine.InputReader.MovementValue.x) < 0.01f && Mathf.Abs(stateMachine.rigidbody2d.velocity.x) < 0.01f)
        {
            stateMachine.SwitchState(new IdlePlayerState(stateMachine));
        }

        if (stateMachine.isWallWalking)
        {
            Vector2 boxSize = new Vector2(1.32f, 0.1f); // Adjust width and height
            Vector2 boxCenterL = new Vector2(stateMachine.transform.position.x - 0.91f, stateMachine.transform.position.y); // Offset
            Vector2 boxCenterR = new Vector2(stateMachine.transform.position.x + 0.91f, stateMachine.transform.position.y); // Offset


            Collider2D hitLeft = Physics2D.OverlapBox(boxCenterL, boxSize, 0f, stateMachine.layerMask);
            Collider2D hitRight = Physics2D.OverlapBox(boxCenterR, boxSize, 0f, stateMachine.layerMask);
            if (hitLeft != null || hitRight != null)
            {
                stateMachine.SwitchState(new WallWalkingState(stateMachine));
            }
            else
            {
                stateMachine.SetWallWalking(false);
            }
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
