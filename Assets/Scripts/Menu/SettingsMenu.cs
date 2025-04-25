using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{

    //Crear un Audio mixer exponer los parametros y cambiar el nombre de la variable expuesta a volume
    [SerializeField] AudioMixer audioMixer;

    Resolution[] resolutions;

    [SerializeField] TMP_Dropdown resolutionDropdown;

    GameObject pauseObject;

    

    
    private void Start()
    {
        pauseObject = transform.GetChild(0).gameObject;
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }
        

        resolutionDropdown.AddOptions(options);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);   
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log(QualitySettings.GetQualityLevel());
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void OnResume()
    {
        GameManager.Instance.SetGamePause(false);
        gameObject.SetActive(false);
        
    }
    public void OnOptions()
    {
        pauseObject.SetActive(true);
        for (int i = 0; i < transform.childCount; i++)
        {
            if(i != 0)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            
        }
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnBack()
    {
        pauseObject.SetActive(false);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i != 0)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }


        }
    }

    public void SetRotationSpeed(float rotationSpeed)
    {
        GameManager.Instance.fixedAnglePerFrame = rotationSpeed;
        GameManager.Instance.RotationSpeed = rotationSpeed;
    }

}
