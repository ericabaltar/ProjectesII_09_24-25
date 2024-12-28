using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidMovement : MonoBehaviour
{
    private Rigidbody2D rigidbody2;
    public float velocity;
    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)) {

            rigidbody2.velocity = new Vector2(-velocity, rigidbody2.velocity.y);
        }
        else if (Input.GetKey(KeyCode.D)) {

            rigidbody2.velocity = new Vector2(velocity, rigidbody2.velocity.y);
        }
        else
            rigidbody2.velocity = new Vector2(0.0f, rigidbody2.velocity.y);

    }
}
