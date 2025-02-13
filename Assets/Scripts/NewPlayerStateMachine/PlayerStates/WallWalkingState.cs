using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WallWalkingState : PlayerBaseState
{
    [field: SerializeField] public ControllerInputSystem InputReader { get; private set; }

    public WallWalkingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        
    }
    float resetGravity = 0f;

    public override void Enter()
    {
        stateMachine.InputReader.RotateLeftEvent += stateMachine.RotateLeft;
        stateMachine.InputReader.RotateRightEvent += stateMachine.RotateRight;
        resetGravity = stateMachine.rigidbody2d.gravityScale;
        stateMachine.rigidbody2d.gravityScale = 0f;
        stateMachine.HaltClimbingAnimation();
        stateMachine.gameObject.GetComponentInChildren<RotationConstraint>().constraintActive = false;
    }

   
    public override void Tick(float deltaTime)
    {

        if (!stateMachine.isWallWalking)
        {
            stateMachine.SwitchState(new MovingPlayerState(stateMachine));
            return;
        }
        

        RaycastHit2D hit1 = Physics2D.Raycast(stateMachine.transform.position, -stateMachine.transform.right, 1.5f, stateMachine.layerMask);

        if (hit1.collider != null)
        {
            if (!stateMachine.particlesLeft.isPlaying)
                stateMachine.particlesLeft.Play();
            hit1.collider.gameObject.transform.GetChild(0).gameObject.SetActive(true);
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

        if (stateMachine.InputReader.MovementValue.y != 0f || stateMachine.InputReader.MovementValue.x != 0f)
        {
            stateMachine.ResumeClimbingAnimation();
        }
        else {
            stateMachine.HaltClimbingAnimation();
        }

        

    }

    public override void FixedTick()
    {
        stateMachine.rigidbody2d.velocity = (new Vector2(0f, stateMachine.InputReader.MovementValue.y) * stateMachine.playerSpeed * Time.fixedDeltaTime * 0.03f);

        if (stateMachine.InputReader.MovementValue.x > 0f || stateMachine.InputReader.MovementValue.x < 0f)
        {
            stateMachine.rigidbody2d.AddForce(new Vector2(stateMachine.InputReader.MovementValue.x, 0f) * stateMachine.playerSpeed * Time.fixedDeltaTime);
        }
    }

    public override void Exit()
    {
        stateMachine.InputReader.RotateLeftEvent -= stateMachine.RotateLeft;
        stateMachine.InputReader.RotateRightEvent -= stateMachine.RotateRight;
        stateMachine.rigidbody2d.gravityScale = resetGravity;
        stateMachine.particlesDown.Stop();
        stateMachine.particlesLeft.Stop();
        stateMachine.particlesRight.Stop();
        stateMachine.particlesUp.Stop();
        stateMachine.RestartAnimationSpeed();
        stateMachine.gameObject.GetComponentInChildren<RotationConstraint>().constraintActive = true;
    }

    //que caiga durante un instante destirotea todo
    //Variables de animación no están coordinadas con los estados
}
