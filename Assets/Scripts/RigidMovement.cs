using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidMovement : MonoBehaviour
{
    private Rigidbody2D rigidbody2;
    public float velocity;

    public GameObject platforms;

    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) 
            rigidbody2.velocity = new Vector2(0.0f, rigidbody2.velocity.y);


        /*if (Input.GetKey(KeyCode.Space)) {
            platforms.transform.parent = GameObject.Find("Main Camera").transform;
            this.gameObject.transform.parent = platforms.transform.parent;
        }
        else if (Input.GetKey(KeyCode.G)) { 
            platforms.transform.parent = GameObject.Find("------Map").transform;
            this.gameObject.transform.parent = platforms.transform.parent;
        }*/
    }
}
