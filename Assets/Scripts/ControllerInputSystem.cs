using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputSystem : MonoBehaviour, InputSystem.IPlayerActions
{

    public Vector2 MovementValue { get; private set; }

    public event Action RotateRightEvent;
    public event Action RotateLeftEvent;
    
    InputSystem controls;

    private void Awake()
    {
        controls = new InputSystem();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
    }

    public void OnMovement(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnRotateLeft(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(!context.performed) { return; }

        RotateLeftEvent?.Invoke();
    }

    public void OnRotateRight(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        
        if (context.performed) { RotateRightEvent?.Invoke(); }

        if(context.canceled)
        { GameManager.Instance.SetRotationState(GameManager.RotationState.ADJUSTING); }
        
    }

}
