using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteringDoorState : PlayerBaseState
{

    [field: SerializeField] public ControllerInputSystem InputReader { get; private set; }

    public EnteringDoorState(PlayerStateMachine stateMachine) : base(stateMachine) { 
    
    }

    public override void Enter()
    {
        stateMachine.anim.SetBool("walking", false);
    }


    public override void Exit() { }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void FixedTick()
    {
        
    }
}
