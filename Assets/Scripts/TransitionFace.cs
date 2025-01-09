using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransitionFace : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        OnStartScene();
    }



    public void OnStartScene()
    {
        transform.DOScale(0, 2f);
        
    }

    public void OnEndScene()
    {
        transform.DOScale(50, 2f);
    }

}
