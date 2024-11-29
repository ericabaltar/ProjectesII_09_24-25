using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class walterscriptdeleteafterfriday : MonoBehaviour
{
    Animator anim;

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
            transform.Rotate(Vector3.forward * -90);
        }
        else if (Input.GetKeyDown(KeyCode.E)) {
            transform.Rotate(Vector3.forward * 90);
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
    }
}
