using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerInputSystem : MonoBehaviour, InputSystem.IPlayerActions
{

    public Vector2 MovementValue { get; private set; }

    public event Action RotateRightEvent;
    public event Action RotateLeftEvent;
    
    InputSystem controls;

    bool restartPress = false;
    bool nextLevelPress = false;
    bool prevLevelPress = false;

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
        if (context.performed) { RotateLeftEvent?.Invoke(); }

        if (context.canceled)
        {
            GameManager.Instance.StopRotation();
            //GameManager.Instance.SetRotationState(GameManager.RotationState.ADJUSTING);
        }

        
    }

    public void OnRotateRight(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        
        if (context.performed) {  RotateRightEvent?.Invoke(); }

        if(context.canceled)
        {
            GameManager.Instance.StopRotation();
            //GameManager.Instance.SetRotationState(GameManager.RotationState.ADJUSTING);
        }
        
    }

    public void OnRestartLevel(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        
        if(context.started)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
    }

    public void OnNextLevel(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(context.started)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void OnPrevLevel(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
           
    }
}
