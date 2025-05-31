using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DelayedExit : MonoBehaviour
{
    public void Exit()
    {
        Invoke("DelayExit", 0.5f);
    }

    private void DelayExit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
