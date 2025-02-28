using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnteringDoorState : PlayerBaseState
{

    [field: SerializeField] public ControllerInputSystem InputReader { get; private set; }

    public EnteringDoorState(PlayerStateMachine stateMachine) : base(stateMachine) { 
    
    }
    Vector3 transformMover;
    private float lerpProgress = 0f;
    public override void Enter()
    {
        stateMachine.anim.SetBool("walking", false);
        //stateMachine.rigidbody2d.velocity = Vector3.zero;
        stateMachine.GetComponent<Collider2D>().enabled = false;
        stateMachine.rigidbody2d.constraints = RigidbodyConstraints2D.None;
        stateMachine.rigidbody2d.freezeRotation = false;
        stateMachine.rigidbody2d.angularVelocity = 340.0f;
        stateMachine.rigidbody2d.angularDrag = 0.1f;
        stateMachine.rigidbody2d.drag = 0.1f;
        transformMover = stateMachine.transform.position;
    }

    public override void Tick(float deltaTime)
    {
        if (lerpProgress < 1f) // Ensure it stops at the door
        {
            lerpProgress += deltaTime; // Increase progress
            stateMachine.transform.position = Vector3.Lerp(transformMover, stateMachine.door.position, lerpProgress);
        }
    }

    public override void FixedTick()
    {
        
    }

    public override void Exit() 
    {
        
    }
}
