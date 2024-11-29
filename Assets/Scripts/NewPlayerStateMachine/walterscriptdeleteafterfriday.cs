using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class walterscriptdeleteafterfriday : MonoBehaviour
{
    private Animator anim;
    private bool canRotate = true;
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
        else
            anim.SetBool("walking",false);

        if (Input.GetKeyDown(KeyCode.Q)) {
            if ((GameObject.Find("------Player").GetComponent<PlayerStateMachine>().isWallWalking ||
                    GameObject.Find("------Player").GetComponent<PlayerStateMachine>().isGrounded) &&
                    GameObject.Find("------Player").GetComponent<Rigidbody2D>().velocity.magnitude < 0.1f &&
                    canRotate) {

                transform.Rotate(Vector3.forward * -90);
                canRotate= false;
                StartCoroutine(WaitForRotation());
            }
                
        }
        else if (Input.GetKeyDown(KeyCode.E)) {
            if ((GameObject.Find("------Player").GetComponent<PlayerStateMachine>().isWallWalking ||
                    GameObject.Find("------Player").GetComponent<PlayerStateMachine>().isGrounded) &&
                    GameObject.Find("------Player").GetComponent<Rigidbody2D>().velocity.magnitude < 0.1f &&
                    canRotate) {

                transform.Rotate(Vector3.forward * 90);
                canRotate= false;
                StartCoroutine(WaitForRotation());
            }
                
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
            SceneManager.LoadScene(1);
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            SceneManager.LoadScene(2);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SceneManager.LoadScene(3);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SceneManager.LoadScene(4);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            SceneManager.LoadScene(5);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            SceneManager.LoadScene(6);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            SceneManager.LoadScene(7);
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            SceneManager.LoadScene(8);
        else if (Input.GetKeyDown(KeyCode.Alpha8))
            SceneManager.LoadScene(9);
        else if (Input.GetKeyDown(KeyCode.Alpha9))
            SceneManager.LoadScene(10);
    }

    IEnumerator WaitForRotation() {

        yield return new WaitForSeconds(1.0f);
        canRotate = true;
    }
}
