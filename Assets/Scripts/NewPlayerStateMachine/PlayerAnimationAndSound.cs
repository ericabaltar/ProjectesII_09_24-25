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
        anim = gameObject.GetComponentInChildren<Animator>();
        mySprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        playerState = gameObject.GetComponentInParent<PlayerStateMachine>();
        myAudioSource = GetComponent<AudioSource>();

        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = GameObject.Find("Main Camera").transform;
        source.weight = 1.0f;
        gameObject.GetComponent<RotationConstraint>().SetSource(0, source);
    }

    
    void Update()
    {
                    
        //evento ejecutable cuando se mueva el jugador
        if (Input.GetKey(KeyCode.D) )
        {
            anim.SetBool("walking", true);
            mySprite.flipX = false;

            if (!myAudioSource.isPlaying && playerState.isGrounded)
            {
                myAudioSource.clip = stepSound;
                myAudioSource.pitch = Random.Range(0.8f, 1.0f);
                myAudioSource.volume = 0.2f;
                myAudioSource.Play();
            }            
        }
        else if (Input.GetKey(KeyCode.A) )
        {
            anim.SetBool("walking", true);
            mySprite.flipX = true;

            if (!myAudioSource.isPlaying && playerState.isGrounded)
            {
                myAudioSource.clip = stepSound;
                myAudioSource.pitch = Random.Range(0.8f, 1.0f);
                myAudioSource.volume = 0.2f;
                myAudioSource.Play();
            }
        }
        else
        {
            anim.SetBool("walking", false);
        }

        //evento animación cuando el jugador detecta no tocar suelo
        if (!playerState.isGrounded && !playerState.isWallWalking)
        {
            anim.SetBool("falling", true);
        }
        //evento sonido cuando el jugador detecta aterrizar
        else if (anim.GetBool("falling") && playerState.isGrounded)
        {
            anim.SetBool("falling", false);
            myAudioSource.clip = landSound;
            myAudioSource.pitch = 1.0f;
            myAudioSource.volume = 1.0f;
            myAudioSource.Play();
        }
        // mantener animación en andar o quieto mientras no caiga
        else 
            anim.SetBool("falling", false);
        


        //cambiar niveles para <-- o -->
        if (Input.GetKeyDown(KeyCode.Alpha0))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else if (Input.GetKeyDown(KeyCode.Alpha9))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
