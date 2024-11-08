using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private float moveHorizontal;
    private float moveVertical;
    private Vector2 currentVelocity;
    [SerializeField] //para poder editar la velocidad desde el editor de unity aunque la variable sea privada
    private float movementSpeed = 3f;
    private Rigidbody2D characterRigidBody;
    public bool isGrounded;

    public Transform particles;

    // Start is called before the first frame update
    private void Start()
    {
        this.characterRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        this.moveHorizontal = Input.GetAxis("Horizontal");
        this.moveVertical = Input.GetAxis("Vertical");
        this.currentVelocity = this.characterRigidBody.velocity;
    }

    private void FixedUpdate()
    {
        if (this.moveHorizontal != 0)
        {
            this.characterRigidBody.velocity = new Vector2(this.moveHorizontal * this.movementSpeed, this.currentVelocity.y);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(transform.CompareTag("Untagged") ||transform.CompareTag("Walkable"))
        {
            isGrounded = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (transform.CompareTag("Untagged") || transform.CompareTag("Walkable"))
        {
            isGrounded = false;
        }
    }

}
