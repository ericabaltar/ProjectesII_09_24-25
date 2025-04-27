using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnHoverSound : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    [SerializeField] private AudioSource audioSource;
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlaySelectSound();
    }

    public void OnSelect(BaseEventData eventData)
    {
        PlaySelectSound();
    }

    private void PlaySelectSound()
    {
        if (EventSystem.current.currentSelectedGameObject != this.gameObject)
            audioSource.Play();
    }
}
