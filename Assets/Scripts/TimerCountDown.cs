using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerCountDown : MonoBehaviour
{
    [SerializeField] float timeToCountDown;

    void Update()
    {
        timeToCountDown -= Time.deltaTime;
        if(timeToCountDown <= 0 )
        {
            SceneManager.LoadScene(0);
        }
    }
}
