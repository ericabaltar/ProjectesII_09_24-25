using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] private string settingsMenuSceneName = "SettingsMenu";

    public void GoToSettingsMenu()
    {

        SceneManager.LoadScene(settingsMenuSceneName);
    }
}
