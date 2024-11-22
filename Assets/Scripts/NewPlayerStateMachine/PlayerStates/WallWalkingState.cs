using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal.ShaderGUI;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WallWalkingState : PlayerBaseState
{
    public WallWalkingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.InputReader.RotateLeftEvent += stateMachine.RotateLeft;
        stateMachine.InputReader.RotateRightEvent += stateMachine.RotateRight;
    }

   
    public override void Tick(float deltaTime)
    {

        if (stateMachine.InputReader.MovementValue.y> 0f || stateMachine.InputReader.MovementValue.y < 0f)
        {
            stateMachine.rigidbody2d.AddForce(new Vector2(0f, stateMachine.InputReader.MovementValue.y) * stateMachine.playerSpeed);
        }

        if (!stateMachine.isWallWalking)
        {
            stateMachine.SwitchState(new MovingPlayerState(stateMachine));
        }
        Debug.Log("IM INSIDE");

        RaycastHit2D hit1 = Physics2D.Raycast(stateMachine.transform.position, -stateMachine.transform.right, 1.5f, stateMachine.layerMask);


        if (hit1.collider != null)
        {
            if (!stateMachine.particlesLeft.isPlaying)
                stateMachine.particlesLeft.Play();
        }
        RaycastHit2D hit2 = Physics2D.Raycast(stateMachine.transform.position, stateMachine.transform.right, 1.5f, stateMachine.layerMask);

        if (hit2.collider != null)
        {
            if (!stateMachine.particlesRight.isPlaying)
                stateMachine.particlesRight.Play();
        }
        RaycastHit2D hit3 = Physics2D.Raycast(stateMachine.transform.position, stateMachine.transform.up, 1.5f, stateMachine.layerMask);

        if (hit3.collider != null)
        {
            if (!stateMachine.particlesUp.isPlaying)
                stateMachine.particlesUp.Play();
        }
        RaycastHit2D hit4 = Physics2D.Raycast(stateMachine.transform.position, -stateMachine.transform.up, 1.5f, stateMachine.layerMask);

        if (hit4.collider != null)
        {
            if (!stateMachine.particlesDown.isPlaying)
                stateMachine.particlesDown.Play();
        }

    }

    public override void Exit()
    {
        stateMachine.InputReader.RotateLeftEvent -= stateMachine.RotateLeft;
        stateMachine.InputReader.RotateRightEvent -= stateMachine.RotateRight;

        stateMachine.particlesDown.Stop();
        stateMachine.particlesLeft.Stop();
        stateMachine.particlesRight.Stop();
        stateMachine.particlesUp.Stop();
    }

}
