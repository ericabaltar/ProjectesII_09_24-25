using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class walterscriptdeleteafterfriday : MonoBehaviour
{
    private Animator anim;
    private bool canRotate = true;
    private bool canStretch = false;
    private bool canSquish = false;

    
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("walking",true);
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("walking",true);
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Input.GetKey(KeyCode.F))
        {
            canSquish = true;
            canStretch = false;
            anim.SetBool("falling",true);
        }
        else if (Input.GetKey(KeyCode.G))
        {            
            anim.SetBool("landing", true);
        }
        else
            anim.SetBool("walking",false);

        if (Input.GetKeyDown(KeyCode.Q)) {
            if ((GameObject.Find("------Player").GetComponent<PlayerStateMachine>().isWallWalking ||
                    GameObject.Find("------Player").GetComponent<PlayerStateMachine>().isGrounded) &&
                    GameObject.Find("------Player").GetComponent<Rigidbody2D>().velocity.magnitude < 0.1f &&
                    canRotate) {

                //transform.Rotate(Vector3.forward * -90);
                canRotate= false;
                StartCoroutine(WaitForRotation());
                StartCoroutine(WaitForStretch());
            }
                
        }
        else if (Input.GetKeyDown(KeyCode.E)) {
            if ((GameObject.Find("------Player").GetComponent<PlayerStateMachine>().isWallWalking ||
                    GameObject.Find("------Player").GetComponent<PlayerStateMachine>().isGrounded) &&
                    GameObject.Find("------Player").GetComponent<Rigidbody2D>().velocity.magnitude < 0.1f &&
                    canRotate) {

                //transform.Rotate(Vector3.forward * 90);
                canRotate= false;
                StartCoroutine(WaitForRotation());
                StartCoroutine(WaitForStretch());
            }
                
        }

        

        if (Input.GetKeyDown(KeyCode.Alpha0))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else if (Input.GetKeyDown(KeyCode.Alpha9))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            anim.SetBool("landing", true);

    }

    IEnumerator WaitForRotation() {

        yield return new WaitForSeconds(1.0f);
        canRotate = true;
        canStretch = true;
    }
    IEnumerator WaitForStretch() {

        yield return new WaitForSeconds(1.0f);

        do
        {
            yield return new WaitForSeconds(0.01f);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + 0.05f, transform.localScale.z);
            transform.localPosition = new Vector3(transform.localPosition.x - 0.02f, transform.localPosition.y, transform.localPosition.z);
        } while (transform.localScale.y < 5.0f && canStretch == true);

        canStretch = false;
    }
}
