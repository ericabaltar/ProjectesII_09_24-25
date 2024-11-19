using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State currentState;
    
    public void SwitchState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    private void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }
}
