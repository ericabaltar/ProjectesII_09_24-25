using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseToggler : MonoBehaviour
{
    private GameObject pauseMenu;
    private bool lastPauseState;

    void Start()
    {
        pauseMenu = transform.GetChild(0).gameObject;
        pauseMenu.SetActive(false);
        lastPauseState = GameManager.Instance.GetGamePause();
        Time.timeScale = lastPauseState ? 0f : 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isPaused = !GameManager.Instance.GetGamePause();
            GameManager.Instance.SetGamePause(isPaused);
            pauseMenu.SetActive(isPaused);
        }

        
        bool currentPauseState = GameManager.Instance.GetGamePause();
        if (currentPauseState != lastPauseState)
        {
            Time.timeScale = currentPauseState ? 0f : 1f;
            lastPauseState = currentPauseState;
        }
    }
}
