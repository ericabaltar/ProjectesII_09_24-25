using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSoundManager : MonoBehaviour
{
    public AudioSource sceneSound; 
    private static bool hasPlayed = false;

    private void Awake()
    {
        
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!hasPlayed && sceneSound != null)
        {
            sceneSound.Play();
            hasPlayed = true; 
        }
    }
}

