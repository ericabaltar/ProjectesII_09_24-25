using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStateMachine : StateMachine
{

    [field: SerializeField] public Rigidbody2D rigidbody2d;
    [field: SerializeField] public ControllerInputSystem InputReader { get; private set; }

    [field: SerializeField] public float playerSpeed = 100f;
    [field: SerializeField] public bool isGrounded { get; private set; } = true;
    [field: SerializeField] public bool isWallWalking { get; private set; } = false;
    [field: SerializeField] public bool isInRotateZone { get; private set; } = false;

    public enum RotationZoneNeeded
    {
        True,
        False
    }

    [field: SerializeField] public RotationZoneNeeded rotationZone { get; private set; } = RotationZoneNeeded.False;

    [Space(10)]
    [Header("UI Transition")]
    public List<MoveUiToCenter> moveUiToCenterList = new List<MoveUiToCenter>();
    public AudioSource sceneSound;
    Scene scene;

    float time = 2f;
    [Space(10)]
    [Header("Particles Part")]
    [Space(10)]
    public LayerMask layerMask;

    public ParticleSystem particlesLeft;
    public ParticleSystem particlesRight;
    public ParticleSystem particlesUp;
    public ParticleSystem particlesDown;

    // Start is called before the first frame update
    void Start()
    {
        SwitchState(new IdlePlayerState(this));
    }



    public void RotateLeft()
    {
        if ((rotationZone == RotationZoneNeeded.False) || (rotationZone == RotationZoneNeeded.True && isInRotateZone))
        {

            if ((isGrounded || isWallWalking) && (rigidbody2d.velocity.magnitude < 0.1f))
            {
                GameManager.Instance.StartRotation(false);
            }
        }
       
    }

    public void RotateRight()
    {
        if((rotationZone == RotationZoneNeeded.False ) || (rotationZone == RotationZoneNeeded.True && isInRotateZone))
        {

            if ((isGrounded || isWallWalking) && (rigidbody2d.velocity.magnitude < 0.1f))
            {
                GameManager.Instance.StartRotation(true);
            }
        }

            
    }


    public void CheckGroundPlayer()
    {
        
        Vector2 boxSize = new Vector2(1f, 0.1f); // Adjust width and height
        Vector2 boxCenter = new Vector2(transform.position.x, transform.position.y - 0.52f); // Offset

     
        Collider2D groundCollider = Physics2D.OverlapBox(boxCenter, boxSize, 0f);

        isGrounded = groundCollider != null;

    }


    private void OnDrawGizmosSelected()
    {
        // Define the size and position of the box
        Vector2 boxSize = new Vector2(1f, 0.1f);
        Vector2 boxCenter = new Vector2(transform.position.x, transform.position.y - 0.5f);

        // Set the Gizmos color
        Gizmos.color = Color.red;

        // Draw the detection box
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if (collision.transform.CompareTag("Untagged") || collision.transform.CompareTag("Walkable"))
        {
            isGrounded = true;
        }
        */
        if (collision.transform.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (collision.transform.CompareTag("Door"))
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
    

        IEnumerator Wait(float time)
        {

            yield return new WaitForSeconds(time);
            scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex + 1);
        }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        /*
        if (collision.transform.CompareTag("Untagged") || collision.transform.CompareTag("Walkable"))
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(1f, 0.1f), 0f);
            isGrounded = false;
            foreach (Collider2D col in colliders)
            {
                if (col.CompareTag("Untagged") || col.CompareTag("Walkable"))
                {
                    isGrounded = true;
                    break;
                }
            }
        }
        */
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Walkable"))
        {
            isWallWalking = true;

        }

        if(collision.CompareTag("RotateZone"))
        {
            isInRotateZone = true;
        }

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Walkable"))
        {
            isWallWalking = false;

        }

        if (collision.CompareTag("RotateZone"))
        {
            isInRotateZone = false;
        }
    }




}
