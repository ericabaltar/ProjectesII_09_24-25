using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Player Part")]
    public bool isGrounded;
    private float moveHorizontal;
    private float moveVertical;
    private Vector2 currentVelocity;
    [SerializeField] //para poder editar la velocidad desde el editor de unity aunque la variable sea privada
    private float movementSpeed = 3f;
    private Rigidbody2D characterRigidBody;
    

    [Space(10)]
    [Header("Particles Part")]
    [Space(10)]
    public LayerMask layerMask;
    

    public ParticleSystem particlesLeft;
    public ParticleSystem particlesRight;
    public ParticleSystem particlesUp;
    public ParticleSystem particlesDown;
    Scene scene;

    float time = 2f;

    public AudioSource sceneSound;

    public bool isWallWalking { get; private set; } = false;

    [Space(10)]
    [Header("UI Transition")]
    public List<MoveUiToCenter> moveUiToCenterList = new List<MoveUiToCenter>();


    Collider2D myCollider;
    // Start is called before the first frame update
    private void Start()
    {
        characterRigidBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        

        //Raycasts

        if(isWallWalking )
        {
            
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position, -transform.right,1.5f,layerMask);
            
            
            if (hit1.collider != null)
            {
                if(!particlesLeft.isPlaying)
                    particlesLeft.Play();
            }

            
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position, transform.right, 1.5f, layerMask);

            if (hit2.collider != null)
            {
                if (!particlesRight.isPlaying)
                    particlesRight.Play();
            }

            RaycastHit2D hit3 = Physics2D.Raycast(transform.position, transform.up, 1.5f, layerMask);

            if (hit3.collider != null)
            {
                if (!particlesUp.isPlaying)
                    particlesUp.Play();
            }

            RaycastHit2D hit4 = Physics2D.Raycast(transform.position, -transform.up, 1.5f, layerMask);

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
        
            if (moveHorizontal != 0)
            {
                characterRigidBody.AddForce(new Vector2(moveHorizontal,0.00f) * movementSpeed,ForceMode2D.Force);
                //this.characterRigidBody.velocity = new Vector2(this.moveHorizontal * this.movementSpeed, this.currentVelocity.y);
            }
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Untagged") || collision.transform.CompareTag("Walkable"))
        {
            isGrounded = true;
        }

        if(collision.transform.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(collision.transform.CompareTag("Door"))
        {

            if (sceneSound != null)
            {
                
                sceneSound.Play();
                foreach (MoveUiToCenter ui in moveUiToCenterList)
                {
                    ui.MoveToCloseCurtains();
                }
             
            }
            StartCoroutine(Wait(time));
           
        }
    }

    IEnumerator Wait(float time)
    {
       
        yield return new WaitForSeconds(time);
        scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex + 1);
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
