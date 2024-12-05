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
    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        mySprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        playerState = gameObject.GetComponentInParent<PlayerStateMachine>();

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
        }
        else if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("walking",true);
            mySprite.flipX = true;
        }
        else if (Input.GetKey(KeyCode.F))
        {
            canSquish = true;
            canStretch = false;
            anim.SetBool("falling",true);
        }
        else if (Input.GetKey(KeyCode.G))
        {            
            anim.SetBool("falling", false);
        }
        else
            anim.SetBool("walking",false);

        if (Input.GetKeyDown(KeyCode.Q)) {
            if (playerState.isWallWalking || playerState.isGrounded) {
                if (canRotate == true)
                {
                    StartCoroutine(WaitForStretch());
                }
            }
                
        }
        else if (Input.GetKeyDown(KeyCode.E)) {
            if (playerState.isWallWalking || playerState.isGrounded) {

                if (canRotate == true)
                {
                    StartCoroutine(WaitForStretch());
                }
            }
                
        }

        if (!playerState.isGrounded && !playerState.isWallWalking)
        {
            anim.SetBool("falling", true);
        }
        else
            anim.SetBool("falling", false);

        if (Input.GetKeyDown(KeyCode.Alpha0))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else if (Input.GetKeyDown(KeyCode.Alpha9))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);        
    }

    
    IEnumerator WaitForStretch() {
        canRotate = true;
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("falling", true);

        canStretch = false;
    }

    IEnumerator Land() {
        anim.SetBool("faling", false);

        yield return new WaitForSeconds(0.1f);

    }
}
