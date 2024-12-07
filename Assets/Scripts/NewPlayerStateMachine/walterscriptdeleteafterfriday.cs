using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class walterscriptdeleteafterfriday : MonoBehaviour
{
    private Animator anim;
    private bool canRotate = true;
    private bool canStretch = true;
    private bool canSquish = false;
    PlayerStateMachine playerState;
    SpriteRenderer mySprite;
    AudioSource myAudioSource;
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

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("walking",true);
            mySprite.flipX = false;
            if (!myAudioSource.isPlaying && playerState.isGrounded)
                myAudioSource.Play();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("walking",true);
            mySprite.flipX = true;
            if (!myAudioSource.isPlaying && playerState.isGrounded)
                myAudioSource.Play();
        }
        else { 
            anim.SetBool("walking",false);
            if (myAudioSource.isPlaying)
                myAudioSource.loop = false;
        }


        if (!playerState.isGrounded && !playerState.isWallWalking)
        {
            anim.SetBool("falling", true);
        }
        else if (anim.GetBool("falling") && playerState.isGrounded) { 
            anim.SetBool("falling", false);
            myAudioSource.Play();
        }
        else
            anim.SetBool("falling", false);


        if (Input.GetKeyDown(KeyCode.Alpha0))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else if (Input.GetKeyDown(KeyCode.Alpha9))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);        
    }

    
    
}
