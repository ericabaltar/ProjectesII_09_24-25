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
        }
        else if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("walking",true);
            Debug.Log(anim.GetBool("walking"));
        }
        else
            anim.SetBool("walking",false);

        if (Input.GetKey(KeyCode.Q)) { 
        
        }
        if (Input.GetKey(KeyCode.E)) { 
        
        }
    }
}
