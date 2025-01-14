using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerStateMachine : StateMachine
{

    [field: SerializeField] public Rigidbody2D rigidbody2d;
    [field: SerializeField] public ControllerInputSystem InputReader { get; private set; }

    [field: SerializeField] public float playerSpeed = 100f;
    [field: SerializeField] public bool isGrounded { get; private set; } = true;
    [field: SerializeField] public bool isWallWalking { get; private set; } = false;
    [field: SerializeField] public bool isInRotateZone { get; private set; } = false;
    public LayerMask groundedLayerMask;

    public enum RotationZoneNeeded
    {
        True,
        False
    }

    [field: SerializeField] public RotationZoneNeeded rotationZone { get; private set; } = RotationZoneNeeded.False;

    [Space(10)]
    [Header("UI Transition")]
    public List<MoveUiToCenter> moveUiToCenterList = new List<MoveUiToCenter>();
    public TransitionFace transitionFace;
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
    public ParticleSystem particlesRunning;
    [Space(10)]
    [Header("Death Part")]
    [Space(10)]
    public ParticleSystem particlesDeath;
    float timeToReset = 2f;

    // Start is called before the first frame update
    void Start()
    {
        SwitchState(new IdlePlayerState(this));
        //tr = GameObject.Find("Square").GetComponent<Transform>();
    }

    private bool canRotate = true;
    private bool canStretch = false;
    private Transform tr;
    public void RotateLeft()
    {
        if ((rotationZone == RotationZoneNeeded.False) || (rotationZone == RotationZoneNeeded.True && isInRotateZone))
        {

            if ((isGrounded || isWallWalking) && (rigidbody2d.velocity.magnitude < 0.1f))
            {
                GameManager.Instance.StartRotation(false);
                //GetComponentInChildren<PlayerAnimationAndSound>().enabled = false;

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
                //GetComponentInChildren<PlayerAnimationAndSound>().enabled = false;

            }
        }

            
    }


    public void CheckGroundPlayer()
    {
        
        Vector2 boxSize = new Vector2(1.32f, 0.1f); // Adjust width and height
        Vector2 boxCenter = new Vector2(transform.position.x, transform.position.y - 0.91f); // Offset

     
        Collider2D groundCollider = Physics2D.OverlapBox(boxCenter, boxSize, 0f,groundedLayerMask);

        isGrounded = groundCollider != null;

    }


    private void OnDrawGizmosSelected()
    {
        Vector2 boxSize = new Vector2(1.32f, 0.1f);
        Vector2 boxCenter = new Vector2(transform.position.x, transform.position.y - 0.91f);

        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(boxCenter, boxSize);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    
        if (collision.transform.CompareTag("Enemy"))
        {
            GetComponentInChildren<RotationConstraint>().enabled = false;
            GetComponentInChildren<Animator>().enabled = false;
            GetComponentInChildren<PlayerAnimationAndSound>().enabled = false;
            GoToDeathState(this);
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

                transitionFace.OnEndScene();

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


    public void GoToDeathState(PlayerStateMachine stateMachine)
    {
        SwitchState(new PlayerDeathState(stateMachine));
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
