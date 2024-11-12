using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Player Part")]
    
    private float moveHorizontal;
    private float moveVertical;
    private Vector2 currentVelocity;
    [SerializeField] //para poder editar la velocidad desde el editor de unity aunque la variable sea privada
    private float movementSpeed = 3f;
    private Rigidbody2D characterRigidBody;
    public bool isGrounded;
    [Space(5)]
    [Header("Particles Part")]
    List<ParticleSystem> particles = new List<ParticleSystem>();
    public LayerMask layerMask;

    public ParticleSystem particlesLeft;
    public ParticleSystem particlesRight;
    public ParticleSystem particlesUp;
    public ParticleSystem particlesDown;

    [SerializeField] bool isWallWalking = false;

    Collider2D myCollider;
    // Start is called before the first frame update
    private void Start()
    {
        this.characterRigidBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        this.moveHorizontal = Input.GetAxis("Horizontal");
        this.moveVertical = Input.GetAxis("Vertical");
        this.currentVelocity = this.characterRigidBody.velocity;

        //Raycasts

        if(isWallWalking )
        {
            
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position, Vector2.left,1.5f,layerMask);
            
            //Debug.DrawRay(transform.position, Vector2.left * 4.5f, Color.red); // Visualize the ray
            if (hit1.collider != null)
            {
                if(!particlesLeft.isPlaying)
                    particlesLeft.Play();
            }


            RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.right, 1.5f, layerMask);

            if (hit2.collider != null)
            {
                if (!particlesRight.isPlaying)
                    particlesRight.Play();
            }

            RaycastHit2D hit3 = Physics2D.Raycast(transform.position, Vector2.up, 1.5f, layerMask);

            if (hit3.collider != null)
            {
                if (!particlesUp.isPlaying)
                    particlesUp.Play();
            }

            RaycastHit2D hit4 = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, layerMask);

            if (hit4.collider != null)
            {
                if (!particlesDown.isPlaying)
                    particlesDown.Play();
            }
        }
        else
        {
            particlesDown.Stop();
            particlesLeft.Stop();
            particlesRight.Stop();
            particlesUp.Stop();
        }


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
        if(collision.transform.CompareTag("Untagged") || collision.transform.CompareTag("Walkable"))
        {
            isGrounded = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Untagged") || collision.transform.CompareTag("Walkable"))
        {
            isGrounded = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Walkable"))
        {
            isWallWalking = true;

        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Walkable"))
        {
            isWallWalking = false;

        }
    }
}
