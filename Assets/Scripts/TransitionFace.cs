using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class TransitionFace : MonoBehaviour
{
    [SerializeField]
    private Material sreenTransitionMaterial;
    
    [SerializeField]
    private float transitionTime = 2.0f;

    private string floatName = "_progress";

    public UnityEvent OnTransitionDone;

    void Start()
    {
        StartCoroutine(TransitionCoroutine());
    }

    private IEnumerator TransitionCoroutine() {
        float currentTime = 0;
        while (currentTime < transitionTime) {
            
            currentTime += Time.deltaTime;
            sreenTransitionMaterial.SetFloat(floatName, Mathf.Clamp01(currentTime/transitionTime));
            yield return null;
        }
        OnTransitionDone?.Invoke();
    }
    private IEnumerator TransitionLeaveCoroutine() {
        float currentTime = 0;
        while (currentTime < transitionTime) {
            
            currentTime += Time.deltaTime;
            sreenTransitionMaterial.SetFloat(floatName, Mathf.Clamp01(1 - currentTime/transitionTime));
            yield return null;
        }
        OnTransitionDone?.Invoke();
    }

    public void OnStartScene()
    {
        //transform.DOScale(0, 2f);
        
    }

    public void OnEndScene()
    {
        //transform.DOScale(50, 2f);
        StartCoroutine(TransitionLeaveCoroutine());
    }

}
