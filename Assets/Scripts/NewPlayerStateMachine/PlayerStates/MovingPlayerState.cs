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

        Debug.Log("MOVING");

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
            

        

        /*
         
        if (Input.GetKey(KeyCode.A)) {

            rigidbody2.velocity = new Vector2(-velocity, rigidbody2.velocity.y);
        }
        else if (Input.GetKey(KeyCode.D)) {

            rigidbody2.velocity = new Vector2(velocity, rigidbody2.velocity.y);
        }
        else
            rigidbody2.velocity = new Vector2(0.0f, rigidbody2.velocity.y);


        stateMachine.rigidbody2d.AddForce(new Vector2(stateMachine.InputReader.MovementValue.x, 0f) * stateMachine.playerSpeed * Time.fixedDeltaTime,ForceMode2D.Force);

        float maxSpeed = stateMachine.playerSpeed; // Define max speed
        stateMachine.rigidbody2d.velocity = new Vector2(
            Mathf.Clamp(stateMachine.rigidbody2d.velocity.x, -maxSpeed, maxSpeed),
            stateMachine.rigidbody2d.velocity.y); // Preserve vertical velocity
        */
    }

    public override void Exit()
    {
        stateMachine.InputReader.RotateLeftEvent -= stateMachine.RotateLeft;
        stateMachine.InputReader.RotateRightEvent -= stateMachine.RotateRight;
    }


}
