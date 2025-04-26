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

    [SerializeField] TMP_Dropdown qualityDropdown;

    GameObject pauseObject;



    private void OnEnable()
    {
        pauseObject = transform.GetChild(0).gameObject;
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            // Buscar la resolución actual
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Configurar calidad
        int currentQualityLevel = QualitySettings.GetQualityLevel();
        qualityDropdown.value = currentQualityLevel;
        qualityDropdown.RefreshShownValue();


        // Player preffs
        if (PlayerPrefs.HasKey("volume"))
        {
            float volume = PlayerPrefs.GetFloat("volume");
            audioMixer.SetFloat("volume", volume);
        }

        if (PlayerPrefs.HasKey("qualityIndex"))
        {
            int qualityIndex = PlayerPrefs.GetInt("qualityIndex");
            QualitySettings.SetQualityLevel(qualityIndex);

            // ⚡ Ahora actualizamos el Dropdown después de aplicar el nivel de calidad
            qualityDropdown.value = qualityIndex;
            qualityDropdown.RefreshShownValue();
        }
        else
        {
            // Si no hay prefs, usamos el valor actual
            qualityDropdown.value = QualitySettings.GetQualityLevel();
            qualityDropdown.RefreshShownValue();
        }

        if (PlayerPrefs.HasKey("isFullScreen"))
        {
            bool isFullScreen = PlayerPrefs.GetInt("isFullScreen") == 1;
            Screen.fullScreen = isFullScreen;
        }

    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("qualityIndex", qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("isFullScreen", isFullScreen ? 1 : 0);
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
