using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGameplay : MonoBehaviour
{
    #region variables
    public enum ButtonDoings
    {
        None,
        Disappear,
        Move,
        Timer
    }

    public ButtonDoings buttonTypes;

    public GameObject objectToEdit;

    [SerializeField] float timer = 0f;
    [SerializeField] float timerReset = 50f;
    [SerializeField] float timeToMove = 40f;
    #endregion


    public void OnPressedButton()
    {
        switch(buttonTypes)
        {
            case ButtonDoings.None:
                break; 
            case ButtonDoings.Disappear:
                objectToEdit.SetActive(false);
                break;
            case ButtonDoings.Move:
                
                break;
            case ButtonDoings.Timer:
                objectToEdit.SetActive(false);
                break;
            default:
                break;
        }


    }
}
