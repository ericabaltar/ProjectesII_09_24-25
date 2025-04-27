using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseToggler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            bool isNowActive = !transform.GetChild(0).gameObject.activeSelf;
            GameManager.Instance.SetGamePause(isNowActive);
            transform.GetChild(0).gameObject.SetActive(isNowActive);

            Time.timeScale = isNowActive ? 0f : 1f;

        }
    }
}
