using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectorManager : MonoBehaviour
{
    //List<Button> levels = new List<Button>();
    
    public void LoadSelectedLevel(int level)
    {
        SceneManager.LoadScene(level);
    }
}
