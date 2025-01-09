using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicGameplay : MonoBehaviour
{
    public static MusicGameplay instance;


    public AudioSource gameMusic;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
        //MIRAD EN EL NIVEL SI YA EXISTE

        gameMusic.Play();
    }
    private void OnLevelWasLoaded()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "SettingsMenu")
        {
            Destroy(gameObject);
        }
    }
}
