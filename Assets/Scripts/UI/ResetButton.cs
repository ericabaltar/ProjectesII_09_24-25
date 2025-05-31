using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    private InputAction resetAction;

    private void OnEnable()
    {
        resetAction = new InputAction(binding: "<Keyboard>/r");
        resetAction.performed += ctx => ReloadScene();
        resetAction.Enable();
    }

    private void OnDisable()
    {
        resetAction.Disable();
    }

    private void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}

