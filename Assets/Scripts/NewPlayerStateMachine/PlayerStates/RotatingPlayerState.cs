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
        
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.GetComponentInChildren<RotationConstraint>().enabled = true;
        if(!GameManager.Instance.isRotating)
        {
            stateMachine.SwitchState(new IdlePlayerState(stateMachine));
        }
    }


    public override void FixedTick()
    {

    }

    public override void Exit()
    {

    }
}
