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
          
            
        }
        else
        {
           
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




   

    

}
