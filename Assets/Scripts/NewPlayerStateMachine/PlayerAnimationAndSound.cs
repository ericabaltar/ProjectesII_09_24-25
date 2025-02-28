using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerAnimationAndSound : MonoBehaviour
{
    private Animator anim;
    private bool canRotate = true;
    private bool canStretch = true;
    private bool canSquish = false;
    PlayerStateMachine playerState;
    SpriteRenderer mySprite;
    AudioSource myAudioSource;
    [field: SerializeField] public AudioClip landSound;
    [field: SerializeField] public AudioClip stepSound;
    void Start()
    {

        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = Camera.main.transform;
        source.weight = 1.0f;
        gameObject.GetComponent<RotationConstraint>().SetSource(0, source);
    }

    
    void Update()
    {
                   //NICE 
        //evento ejecutable cuando se mueva el jugador
       

        //evento animación cuando el jugador detecta no tocar suelo
       


        //cambiar niveles para <-- o -->
        if (Input.GetKeyDown(KeyCode.Alpha0))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else if (Input.GetKeyDown(KeyCode.Alpha9))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
