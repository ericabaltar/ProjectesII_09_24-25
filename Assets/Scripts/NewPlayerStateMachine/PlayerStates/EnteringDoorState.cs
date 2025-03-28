using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Animations;

public class EnteringDoorState : PlayerBaseState
{

    [field: SerializeField] public ControllerInputSystem InputReader { get; private set; }

    public EnteringDoorState(PlayerStateMachine stateMachine) : base(stateMachine) { 
    
    }
    Vector3 transformMover;
    private float lerpProgress = 1.2f;
    bool isComplete = false;
    public override void Enter()
    {
        stateMachine.rigidbody2d.velocity = Vector3.zero;
        stateMachine.rigidbody2d.gravityScale = 0;
        stateMachine.GetComponentInChildren<RotationConstraint>().enabled = false;
        stateMachine.anim.SetBool("walking", false);
        stateMachine.anim.SetTrigger("enteringDoor");
       
        stateMachine.GetComponent<Collider2D>().enabled = false;
        stateMachine.rigidbody2d.constraints = RigidbodyConstraints2D.None;
        stateMachine.rigidbody2d.freezeRotation = false;
        stateMachine.rigidbody2d.angularVelocity = 340.0f;
        stateMachine.rigidbody2d.angularDrag = 0.1f;
        stateMachine.rigidbody2d.drag = 0.1f;
        transformMover = stateMachine.transform.position;
        stateMachine.transform.DOMove(stateMachine.door.position, lerpProgress)
            .SetEase(Ease.InSine).OnComplete(() => {
                isComplete = true;
            });
        
        
    }

    public override void Tick(float deltaTime)
    {
        
       
        if(isComplete)
        {
            Color newColor = stateMachine.mySprite.color;
            newColor.a = 0f;
            stateMachine.mySprite.DOColor(newColor,lerpProgress);
            isComplete = false;
        }

        
    }

    public override void FixedTick()
    {
        
    }

    public override void Exit() 
    {
        
    }
}
