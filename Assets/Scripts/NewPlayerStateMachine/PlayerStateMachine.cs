using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerStateMachine : StateMachine
{
    private float separationOfFloorChecker = 1.3f;

    public Material glowMaterial;

    [field: SerializeField] public Rigidbody2D rigidbody2d;
    [field: SerializeField] public ControllerInputSystem InputReader { get; private set; }

    [field: SerializeField] public float playerSpeed = 100f;
    [field: SerializeField] public bool isGrounded { get; private set; } = true;
    [field: SerializeField] public bool isWallWalking { get; private set; } = false;
    bool safety_isWallWalking = false;
    [field: SerializeField] public bool isInRotateZone { get; private set; } = false;
    [field: SerializeField] public bool rotateOutsideZone { get; private set; } = false;
    //ANIMATIONS PLAYER

    public SpriteRenderer mySprite;
    public Animator anim;
    [field: SerializeField] public AudioClip landSound;
    [field: SerializeField] public AudioClip stepSound;
    AudioSource myAudioSource;

    public ParticleSystem ps;

    //DOOOR ENTER 
    [NonSerialized] public Transform door;
    
    //DOOR FINISH


    //END ANIMATIONS PLAYER

    public LayerMask groundedLayerMask;

    public enum RotationZoneNeeded
    {
        True,
        False
    }

    public enum OutSideZoneNeeded
    {
        True,
        False
    }

    [field: SerializeField] public RotationZoneNeeded rotationZone { get; private set; } = RotationZoneNeeded.False;
    [field: SerializeField] public OutSideZoneNeeded outsideRotationZone { get; private set; } = OutSideZoneNeeded.False;

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
    public AudioClip ouchSound;
    float timeToReset = 2f;

    //SAVE RAYCAST WALL
    public GameObject hit1;
    public GameObject hit2;
    public GameObject hit3;
    public GameObject hit4;


    RaycastHit2D groundCheck1;
    RaycastHit2D groundCheck2;
    private void Awake()
    {
        myAudioSource = GetComponentInChildren<AudioSource>();
        mySprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        if(outsideRotationZone == OutSideZoneNeeded.True)            
            GameManager.Instance.objectsToConsider.Remove(gameObject);
        */
        SwitchState(new IdlePlayerState(this));
    }

    private bool canRotate = true;
    private bool canStretch = false;
    public void RotateLeft()
    {
        if ((rotationZone == RotationZoneNeeded.False) || (rotationZone == RotationZoneNeeded.True && isInRotateZone) || (outsideRotationZone == OutSideZoneNeeded.True))
        {

            if ((isGrounded || isWallWalking) && (rigidbody2d.velocity.magnitude < 0.1f))
            {
                GoToRotateState();
                GameManager.Instance.StartRotation(false);
                //GetComponentInChildren<PlayerAnimationAndSound>().enabled = false;

            }
        }
       
    }

    public void RotateRight()
    {
        if((rotationZone == RotationZoneNeeded.False ) || (rotationZone == RotationZoneNeeded.True && isInRotateZone) || (outsideRotationZone == OutSideZoneNeeded.True))
        {

            if ((isGrounded || isWallWalking) && (rigidbody2d.velocity.magnitude < 0.1f))
            {
                GoToRotateState();
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

        isGrounded = groundCollider != null; /*&& (groundCollider.CompareTag("Walkable") 
                                                || groundCollider.CompareTag("Untagged") 
                                                || groundCollider.CompareTag("Box"));*/

        groundCheck1 = Physics2D.Raycast(new Vector2(transform.position.x - separationOfFloorChecker, transform.position.y), -transform.up, 1.5f, layerMask);
        groundCheck2 = Physics2D.Raycast(new Vector2(transform.position.x + separationOfFloorChecker, transform.position.y), -transform.up, 1.5f, layerMask);
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

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
       
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Walkable"))
        {
            if (groundCheck1.collider != null || groundCheck2.collider != null)
            {
                
            isWallWalking = true;
            safety_isWallWalking = true;
            }
        }

        if (collision.CompareTag("RotateZone"))
        {
            isInRotateZone = true;
            if (outsideRotationZone == OutSideZoneNeeded.True && GameManager.Instance.rotationState == GameManager.RotationState.IDLE)
            {
                if(!GameManager.Instance.objectsToConsider.Contains(gameObject))
                    GameManager.Instance.objectsToConsider.Add(gameObject);
            }
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
                door = collision.transform;
                SwitchState(new EnteringDoorState(this));
                gameObject.GetComponent<TransitionFace>().ShrinkCircle();
            }
            StartCoroutine(Wait(time));

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Walkable"))
        {
            safety_isWallWalking = false;

            //isWallWalking = false;
        }

        if (collision.CompareTag("RotateZone"))
        {
            isInRotateZone = false;
            if (outsideRotationZone == OutSideZoneNeeded.True && GameManager.Instance.rotationState == GameManager.RotationState.IDLE)
            {
                if (GameManager.Instance.objectsToConsider.Contains(gameObject))
                    GameManager.Instance.objectsToConsider.Remove(gameObject);
            }
        }
    }

    // Enable emission and assign glowMaterial
    private void EnableEmission(GameObject obj, Material glowMat)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null && glowMat != null)
        {
            renderer.material = glowMat; // Assign the material
            renderer.material.EnableKeyword("_EMISSION");
        }
    }

    // Disable emission by reverting to the default material
    private void DisableEmission(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.DisableKeyword("_EMISSION");
            renderer.material.SetColor("_EmissionColor", Color.black); // Remove glow effect
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

    public void AnimateAndRotateAccording()
    {

        if (InputReader.MovementValue.x >0f)
        {
            anim.SetBool("walking", true);
            mySprite.flipX = false;

            if (!myAudioSource.isPlaying && isGrounded)
            {
                myAudioSource.clip = stepSound;
                myAudioSource.pitch = UnityEngine.Random.Range(0.8f, 1.0f);
                myAudioSource.volume = 0.2f;
                myAudioSource.Play();
            }
        }
        else if (InputReader.MovementValue.x < 0f)
        {
            anim.SetBool("walking", true);
            mySprite.flipX = true;

            if (!myAudioSource.isPlaying && isGrounded)
            {
                myAudioSource.clip = stepSound;
                myAudioSource.pitch = UnityEngine.Random.Range(0.8f, 1.0f);
                myAudioSource.volume = 0.2f;
                myAudioSource.Play();
            }
        }
        else if (isWallWalking && !isGrounded)
        {
            anim.SetBool("climbing", true);
            anim.SetBool("falling", false);
            //gameObject.GetComponentInChildren<RotationConstraint>().constraintActive = false;
        }
        else
        {
            anim.SetBool("walking", false);
        }
    }

    public void StretchAndSquash()
    {
        if (!isGrounded && !isWallWalking)
        {
            anim.SetBool("falling", true);
            anim.SetBool("climbing", false);            
        }
        //evento sonido cuando el jugador detecta aterrizar
        else if (anim.GetBool("falling") && isGrounded)
        {
            anim.SetBool("falling", false);
            myAudioSource.clip = landSound;
            myAudioSource.pitch = 1.0f;
            myAudioSource.volume = 1.0f;
            myAudioSource.Play();
            ps.Emit(5);
        }
        //evento animaci?n cuando el jugador detecta escalar
        else if (isWallWalking && !isGrounded)
        {
            anim.SetBool("climbing", true);
            anim.SetBool("falling", false);            
        }
        // mantener animaci?n en andar o quieto mientras no caiga
        else
            anim.SetBool("falling", false);

    }

    public void HaltClimbingAnimation() {

        anim.speed = 0.0f;
    }
    public void ResumeClimbingAnimation()
    {

        anim.speed = 2.0f;
    }
    public void RestartAnimationSpeed()
    {

        anim.speed = 1.0f;
    }

    public void GoToRotateState()
    {
        //SwitchState(new RotatingPlayerState(this));
    }

    public void SetWallWalking(bool b){
        isWallWalking = b;
    }
    public bool GetSafety(){
        return safety_isWallWalking;
    }
    IEnumerator Wait(float time)
    {

        yield return new WaitForSeconds(time);
        scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex + 1);
    }
}
