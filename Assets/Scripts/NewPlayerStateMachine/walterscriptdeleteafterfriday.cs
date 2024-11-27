using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Debug.Log(anim.GetBool("walking"));
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("walking",true);
            Debug.Log(anim.GetBool("walking"));
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
            anim.SetBool("walking",false);

        if (Input.GetKeyDown(KeyCode.Q)) {
            transform.Rotate(Vector3.forward * -90);
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            transform.Rotate(Vector3.forward * 90);
        }
    }
}
