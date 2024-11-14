using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveUiToCenter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //transform.DOMoveX(transform.position.x/3, 1); //When leaving the level
        transform.DOMoveX(transform.position.x*3, 1); //At start of the level
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
