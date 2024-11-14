using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveUiToCenter : MonoBehaviour
{
    void Start()
    {
        
        transform.DOMoveX(transform.position.x*5, 1);
    }

    public void MoveToCloseCurtains()
    {
        transform.DOMoveX(transform.position.x/5, 1); 
    }
}
