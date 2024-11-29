using UnityEngine;

public class SoundManager : MonoBehaviour
{
    void OnEnable()
    {
        
        ToggleButton.ToggleStateChanged += ToggleSound;
    }

    void OnDisable()
    {
        
        ToggleButton.ToggleStateChanged -= ToggleSound;
    }

    
    private void ToggleSound(bool isSoundActive)
    {
       
        AudioListener.pause = !isSoundActive;
    }
}

