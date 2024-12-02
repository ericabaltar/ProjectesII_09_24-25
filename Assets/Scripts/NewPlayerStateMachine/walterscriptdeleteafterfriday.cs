using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class walterscriptdeleteafterfriday : MonoBehaviour
{
    private Animator anim;
    private bool canRotate = true;

    private float initialStretchValue = 1.0f;
    private float maximumStretchValue = 1.0f;

    [SerializeField] private AnimationCurve squashAndStretchCurve = new AnimationCurve(
        new Keyframe(0, 0.0f),
        new Keyframe(0.25f, 1.0f),
        new Keyframe(1.0f, 0.0f)
        )
        ;
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else if (Input.GetKeyDown(KeyCode.Alpha9))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        
    }

    IEnumerator WaitForRotation() {

        yield return new WaitForSeconds(1.0f);
        canRotate = true;
    }
}
