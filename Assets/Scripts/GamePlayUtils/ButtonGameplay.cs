using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGameplay : MonoBehaviour
{
    [SerializeField] private GameObject door; 
    [SerializeField] private SpriteRenderer buttonSpriteRenderer; 
    [SerializeField] private Sprite pressedSprite; 
    [SerializeField] private Sprite defaultSprite; 

    private int pressCount = 0;



    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            pressCount++;
            if (pressCount == 1) 
            {
                OnButtonPressed();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            pressCount = Mathf.Max(0, pressCount - 1);
            if (pressCount == 0) 
            {
                OnButtonReleased();
            }
        }
    }

    private void OnButtonPressed()
    {
        if (door != null)
        {
            door.SetActive(true);
        }

        if (buttonSpriteRenderer != null && pressedSprite != null)
        {
            buttonSpriteRenderer.sprite = pressedSprite;
        }
    }

    private void OnButtonReleased()
    {
        if (door != null)
        {
            door.SetActive(false);
        }

        if (buttonSpriteRenderer != null && defaultSprite != null)
        {
            buttonSpriteRenderer.sprite = defaultSprite;
        }
    }
}

