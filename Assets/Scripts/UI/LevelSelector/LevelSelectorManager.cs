using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectorManager : MonoBehaviour
{
    public Transform content; // The parent object where buttons will be added.
    public Button buttonPrefab; 

    void Start()
    {
        PopulateLevelButtons();
    }

    void PopulateLevelButtons()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int numberScene = SceneManager.GetActiveScene().buildIndex +1;
        int buttonNumber = 1; // Start number at 1
        for (int i = numberScene;  i < sceneCount-1; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i); 
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath); 

            
            Button newButton = Instantiate(buttonPrefab, content);

            TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = buttonNumber.ToString();
            }
            else
            {
                Debug.LogError("Button prefab is missing a TMP_Text component!");
            }

            
            int index = i; 
            newButton.onClick.AddListener(() => LoadSelectedLevel(index));

            buttonNumber++;
        }
    }

    public void LoadSelectedLevel(int level)
    {
        SceneManager.LoadScene(level);
    }
}
