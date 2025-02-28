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
        
        for (int i = 3; i < sceneCount-1; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i); 
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath); 

            
            Button newButton = Instantiate(buttonPrefab, content);

            TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = i.ToString();
            }
            else
            {
                Debug.LogError("Button prefab is missing a TMP_Text component!");
            }

            
            int index = i; 
            newButton.onClick.AddListener(() => LoadSelectedLevel(index));
        }
    }

    public void LoadSelectedLevel(int level)
    {
        SceneManager.LoadScene(level);
    }
}
