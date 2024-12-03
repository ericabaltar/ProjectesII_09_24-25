using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathState : PlayerBaseState
{
    public PlayerDeathState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }
    float timeToRestart = 2f;
    float pushUp = 20f;
    float pushToSide = 20f;
    public override void Enter()
    {
        stateMachine.particlesDeath.Play();
        stateMachine.GetComponent<Collider2D>().enabled = false;
        int direction = Random.Range(0, 2);
        stateMachine.rigidbody2d.AddForce(new Vector2(direction == 0 ? pushToSide : -pushToSide, pushUp), ForceMode2D.Impulse);
    }

    public override void Tick(float deltaTime)
    {
        timeToRestart -= Time.deltaTime;
        stateMachine.transform.Rotate(Vector3.forward, 340f * deltaTime);
        if(timeToRestart <= 0f) 
        {
            stateMachine.RestartLevel();
        }
        //Death
    }


    public override void FixedTick()
    {
        
    }

    public override void Exit()
    {
        
    }
}
