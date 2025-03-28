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
            GameManager.Instance.SetGamePause(!GameManager.Instance.GetGamePause());
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
            
        }
    }
}
