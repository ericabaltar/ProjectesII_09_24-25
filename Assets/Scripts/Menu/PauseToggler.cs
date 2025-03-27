using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseToggler : MonoBehaviour
{

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.SetGamePause(!GameManager.Instance.GetGamePause());
            if(GameManager.Instance.GetGamePause() )
            {
                Time.timeScale = 0.0000f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
            
        }
    }
}
