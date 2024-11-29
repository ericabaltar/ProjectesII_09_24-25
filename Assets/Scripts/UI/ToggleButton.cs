using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    
    public Sprite activeSprite;   
    public Sprite inactiveSprite; 

    
    private Image buttonImage;

    
    private bool isActive = false;

   
    private const string BUTTON_STATE_KEY = "ButtonState";

    
    public delegate void OnToggleStateChanged(bool isActive);
    public static event OnToggleStateChanged ToggleStateChanged;

    void Start()
    {
        
        buttonImage = GetComponent<Image>();

        
        isActive = PlayerPrefs.GetInt(BUTTON_STATE_KEY, 1) == 1; 

      
        UpdateButtonSprite();

       
        ToggleStateChanged?.Invoke(isActive);
    }

    
    public void ToggleState()
    {
        isActive = !isActive;
        UpdateButtonSprite(); 

       
        PlayerPrefs.SetInt(BUTTON_STATE_KEY, isActive ? 1 : 0);
        PlayerPrefs.Save();

       
        ToggleStateChanged?.Invoke(isActive);
    }

   
    private void UpdateButtonSprite()
    {
        buttonImage.sprite = isActive ? activeSprite : inactiveSprite;
    }
}
