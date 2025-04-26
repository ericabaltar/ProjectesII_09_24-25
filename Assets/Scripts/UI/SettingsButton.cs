using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] private string settingsMenuSceneName = "SettingsMenu";

    [SerializeField] Transform canvasMenu;

    public void GoToSettingsMenu()
    {

        canvasMenu.gameObject.SetActive(!canvasMenu.gameObject.activeSelf);
    }
}
