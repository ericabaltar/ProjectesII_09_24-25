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
       


        if ((stateMachine.InputReader.MovementValue.x > 0.0f || stateMachine.InputReader.MovementValue.x < 0.0f) && (stateMachine.rigidbody2d.velocity.magnitude < 0.0f) || (stateMachine.rigidbody2d.velocity.magnitude > 0.0f))
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
        stateMachine.rigidbody2d.AddForce(new Vector2(stateMachine.InputReader.MovementValue.x, 0f) * stateMachine.playerSpeed * Time.fixedDeltaTime,ForceMode2D.Force);

        float maxSpeed = stateMachine.playerSpeed; // Define max speed
        stateMachine.rigidbody2d.velocity = new Vector2(
            Mathf.Clamp(stateMachine.rigidbody2d.velocity.x, -maxSpeed, maxSpeed),
            stateMachine.rigidbody2d.velocity.y); // Preserve vertical velocity
    }

    public override void Exit()
    {
        stateMachine.InputReader.RotateLeftEvent -= stateMachine.RotateLeft;
        stateMachine.InputReader.RotateRightEvent -= stateMachine.RotateRight;
    }


}
