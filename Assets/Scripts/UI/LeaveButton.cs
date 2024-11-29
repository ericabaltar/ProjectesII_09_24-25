using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveButton : MonoBehaviour
{
    
    [SerializeField] private string mainMenuSceneName = "MainMenu";

   
    public void GoToMainMenu()
    {
        
        SceneManager.LoadScene(mainMenuSceneName);
    }
}

