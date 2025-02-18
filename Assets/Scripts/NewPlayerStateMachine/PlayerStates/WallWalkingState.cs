using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WallWalkingState : PlayerBaseState
{
    float wallRaycastDistance = 1.3f;

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

        //Debug.Log("Wall Walking State");

        stateMachine.anim.SetBool("falling", false);
        stateMachine.anim.SetBool("walking", false);
        stateMachine.anim.SetBool("climbing", true);
        //Debug.Log("CLIMB!!!!!!!!!!");

        Vector2 boxSize = new Vector2(1.32f, 0.1f); // Adjust width and height
        Vector2 boxCenterL = new Vector2(stateMachine.transform.position.x - 0.91f, stateMachine.transform.position.y); // Offset
        Vector2 boxCenterR = new Vector2(stateMachine.transform.position.x + 0.91f, stateMachine.transform.position.y); // Offset

        Collider2D hitLeft = Physics2D.OverlapBox(boxCenterL, boxSize, 0f, stateMachine.layerMask);
        Collider2D hitRight = Physics2D.OverlapBox(boxCenterR, boxSize, 0f, stateMachine.layerMask);

        if (hitLeft != null)
            stateMachine.mySprite.flipX = true;
        else if (hitRight != null)
            stateMachine.mySprite.flipX = false;

        stateMachine.particlesRunning.Stop();
    }


    public override void Tick(float deltaTime)
    {

        RaycastHit2D hit1 = Physics2D.Raycast(stateMachine.transform.position, -stateMachine.transform.right, wallRaycastDistance, stateMachine.layerMask);

        if (hit1.collider != null)
        {
            if (!stateMachine.particlesLeft.isPlaying)
                stateMachine.particlesLeft.Play();
           
            stateMachine.hit1 = hit1.collider.gameObject.transform.GetChild(0).gameObject;
            stateMachine.hit1.SetActive(true);
        }
        

        RaycastHit2D hit2 = Physics2D.Raycast(stateMachine.transform.position, stateMachine.transform.right, wallRaycastDistance, stateMachine.layerMask);

        if (hit2.collider != null)
        {
            if (!stateMachine.particlesRight.isPlaying)
                stateMachine.particlesRight.Play();
            stateMachine.hit2 = hit2.collider.gameObject.transform.GetChild(0).gameObject;
            stateMachine.hit2.SetActive(true);
        }
        

        RaycastHit2D hit3 = Physics2D.Raycast(stateMachine.transform.position, stateMachine.transform.up, wallRaycastDistance, stateMachine.layerMask);

        if (hit3.collider != null)
        {
            if (!stateMachine.particlesUp.isPlaying)
                stateMachine.particlesUp.Play();

            stateMachine.hit3 = hit3.collider.gameObject.transform.GetChild(0).gameObject;
            stateMachine.hit3.SetActive(true);
            
        }
        


        RaycastHit2D hit4 = Physics2D.Raycast(stateMachine.transform.position, -stateMachine.transform.up, wallRaycastDistance, stateMachine.layerMask);
        
        if (hit4.collider != null)
        {
            if (!stateMachine.particlesDown.isPlaying)
                stateMachine.particlesDown.Play();

            stateMachine.hit4 = hit4.collider.gameObject.transform.GetChild(0).gameObject;
            stateMachine.hit4.SetActive(true);
        }




        if (stateMachine.InputReader.MovementValue.y != 0f || stateMachine.InputReader.MovementValue.x != 0f)
        {
            stateMachine.ResumeClimbingAnimation();
        }
        else {
            stateMachine.HaltClimbingAnimation();
        }

        if (!stateMachine.isWallWalking)
        {
            stateMachine.SwitchState(new MovingPlayerState(stateMachine));
            return;
        }


        if (hit1.collider == null 
            && hit2.collider == null 
            && hit3.collider == null 
            && hit4.collider == null
            && stateMachine.GetSafety() == false)
        {

            stateMachine.anim.SetBool("falling", true);
            stateMachine.anim.SetBool("walking", false);
            stateMachine.anim.SetBool("climbing", false);
            stateMachine.gameObject.GetComponentInChildren<RotationConstraint>().constraintActive = true;
            stateMachine.SetWallWalking(false);
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

        if (stateMachine.hit1 != null)
        {
            Debug.Log("false");
            stateMachine.hit1.SetActive(false);
            stateMachine.hit1 = null;
        }
        else if (stateMachine.hit2 != null)
        {
            stateMachine.hit2.SetActive(false);
            stateMachine.hit2 = null;
        }
        else if (stateMachine.hit3 != null)
        {
            stateMachine.hit3.SetActive(false);
            stateMachine.hit3 = null;
        }
        else if (stateMachine.hit4 != null)
        {
            stateMachine.hit4.SetActive(false);
            stateMachine.hit4 = null;
        }

    }

    //que caiga durante un instante destirotea todo
    //Variables de animación no están coordinadas con los estados
}
