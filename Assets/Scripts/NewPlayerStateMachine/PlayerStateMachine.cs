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
    #region Serialized Fields
    [Header("Player Components")]
    [field: SerializeField] public Rigidbody2D rigidbody2d;
    [field: SerializeField] public ControllerInputSystem InputReader { get; private set; }

    [Header("Player Movement Settings")]
    [field: SerializeField] public float playerSpeed = 100f;

    [Header("Ground & Wall Detection")]
    [field: SerializeField] public LayerMask groundedLayerMask;
    [field: SerializeField] public LayerMask layerMask;
    [field: SerializeField] public bool isGrounded { get; private set; } = true;
    [field: SerializeField] public bool isWallWalking { get; private set; } = false;
    [field: SerializeField] public bool isInRotateZone { get; private set; } = false;
    [field: SerializeField] public bool rotateOutsideZone { get; private set; } = false;
    #endregion

    #region Enums
    public enum RotationZoneNeeded { True, False, Worlds }
    public enum OutSideZoneNeeded { True, False, NoRotationZone }

    [field: SerializeField] public RotationZoneNeeded rotationZone { get; private set; } = RotationZoneNeeded.False;
    [field: SerializeField] public OutSideZoneNeeded outsideRotationZone { get; private set; } = OutSideZoneNeeded.False;
    #endregion

    #region Animation & Audio
    public SpriteRenderer mySprite;
    public Animator anim;
    public AudioSource sceneSound;
    AudioSource myAudioSource;

    [field: SerializeField] public AudioClip landSound;
    [field: SerializeField] public AudioClip stepSound;
    [field: SerializeField] public AudioClip ouchSound;
    #endregion

    #region Particle Systems
    public ParticleSystem ps;
    public ParticleSystem particlesLeft;
    public ParticleSystem particlesRight;
    public ParticleSystem particlesUp;
    public ParticleSystem particlesDown;
    public ParticleSystem particlesRunning;
    public ParticleSystem particlesDeath;
    #endregion

    #region Visual FX & Utilities
    public Material glowMaterial;
    public GameObject spriteholder;
    public GameObject hit1, hit2, hit3, hit4;
    public Transform door;
    #endregion

    #region Private Fields
    private float timeToReset = 2f;
    private float separationOfFloorChecker = 1.3f;
    private bool safety_isWallWalking = false;
    private bool isDead = false;
    private bool canRotate = true;
    private bool canStretch = false;
    private Scene scene;
    private float time = 2f;

    private RaycastHit2D groundCheck1;
    private RaycastHit2D groundCheck2;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        myAudioSource = GetComponentInChildren<AudioSource>();
        mySprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        SwitchState(new IdlePlayerState(this));
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 boxSize = new(1.32f, 0.1f);
        Vector2 boxCenter = new(transform.position.x, transform.position.y - 0.91f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
    #endregion

    #region Rotation Control
    public void RotateLeft() => TryRotate(false);
    public void RotateRight() => TryRotate(true);

    private void TryRotate(bool toRight)
    {
        if (IsRotationAllowed() && (isGrounded || isWallWalking) && rigidbody2d.velocity.magnitude < 0.1f)
        {
            GoToRotateState();
            GameManager.Instance.StartRotation(toRight);
        }
    }

    private bool IsRotationAllowed()
    {
        return rotationZone == RotationZoneNeeded.False ||
               (rotationZone == RotationZoneNeeded.True && isInRotateZone) ||
               outsideRotationZone == OutSideZoneNeeded.True;
    }
    #endregion

    #region Ground & Wall Detection
    public void CheckGroundPlayer()
    {
        Vector2 boxSize = new(1.32f, 0.1f);
        Vector2 boxCenter = new(transform.position.x, transform.position.y - 0.91f);

        Collider2D groundCollider = Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundedLayerMask);
        isGrounded = groundCollider != null;

        groundCheck1 = Physics2D.Raycast(new Vector2(transform.position.x - separationOfFloorChecker, transform.position.y), -transform.up, 1.5f, layerMask);
        groundCheck2 = Physics2D.Raycast(new Vector2(transform.position.x + separationOfFloorChecker, transform.position.y), -transform.up, 1.5f, layerMask);
    }

    public void SetWallWalking(bool state) => isWallWalking = state;
    public bool GetSafety() => safety_isWallWalking;
    public bool IsDead() => isDead;
    #endregion

    #region Collision & Trigger Handlers
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            DisableAllAnimation();
            GoToDeathState(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Walkable":
                HandleWalkableEnter();
                break;
            case "RotateZone":
                HandleRotateZoneEnter();
                break;
            case "NoRotateZone":
                HandleNoRotateZoneEnter();
                break;
            case "Door":
                HandleDoorEnter(collision.transform);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Walkable":
                HandleWalkableExit();
                break;
            case "RotateZone":
                HandleRotateZoneExit();
                break;
            case "NoRotateZone":
                HandleNoRotateZoneExit();
                break;
        }
    }

    private void HandleWalkableEnter()
    {
        if (groundCheck1.collider != null || groundCheck2.collider != null)
        {
            isWallWalking = true;
            safety_isWallWalking = true;
        }
        GetComponentInChildren<RotationConstraint>().constraintActive = false;
    }

    private void HandleWalkableExit()
    {
        safety_isWallWalking = false;
        if (InputReader.MovementValue.x != 0f)
            GetComponentInChildren<RotationConstraint>().constraintActive = true;
    }

    private void HandleRotateZoneEnter()
    {
        isInRotateZone = true;
        if (outsideRotationZone == OutSideZoneNeeded.True && GameManager.Instance.rotationState == GameManager.RotationState.IDLE)
        {
            if (!GameManager.Instance.objectsToConsider.Contains(gameObject))
                GameManager.Instance.objectsToConsider.Add(gameObject);
        }
    }

    private void HandleRotateZoneExit()
    {
        isInRotateZone = false;
        if (outsideRotationZone == OutSideZoneNeeded.True && GameManager.Instance.rotationState == GameManager.RotationState.IDLE)
            GameManager.Instance.objectsToConsider.Remove(gameObject);
    }

    private void HandleNoRotateZoneEnter()
    {
        isInRotateZone = false;
        if (outsideRotationZone == OutSideZoneNeeded.NoRotationZone && GameManager.Instance.rotationState == GameManager.RotationState.IDLE)
        {
            if (GameManager.Instance.objectsToConsider.Contains(gameObject))
                GameManager.Instance.objectsToConsider.Remove(gameObject);
        }
    }

    private void HandleNoRotateZoneExit()
    {
        isInRotateZone = true;
        if (outsideRotationZone == OutSideZoneNeeded.NoRotationZone && GameManager.Instance.rotationState == GameManager.RotationState.IDLE)
        {
            if (!GameManager.Instance.objectsToConsider.Contains(gameObject))
                GameManager.Instance.objectsToConsider.Add(gameObject);
        }
    }

    private void HandleDoorEnter(Transform doorTransform)
    {
        if (sceneSound != null)
        {
            sceneSound.Play();
            door = doorTransform;
            SwitchState(new EnteringDoorState(this));
            GetComponent<TransitionFace>().ShrinkCircle();
            StartCoroutine(Wait(time));
        }
    }
    #endregion

    #region States
    public void GoToDeathState(PlayerStateMachine stateMachine)
    {
        isDead = true;
        SwitchState(new PlayerDeathState(stateMachine));
    }

    public void GoToRotateState()
    {
        //SwitchState(new RotatingPlayerState(this));
    }

    IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    #endregion

    #region Animation & Sound Logic
    public void AnimateAndRotateAccording()
    {
        float moveX = InputReader.MovementValue.x;
        bool shouldFlip = moveX < 0;

        if (moveX != 0f)
        {
            anim.SetBool("walking", true);
            mySprite.flipX = shouldFlip;
            PlayStepSound();
        }
        else if (isWallWalking && !isGrounded)
        {
            anim.SetBool("climbing", true);
            anim.SetBool("falling", false);
        }
        else
        {
            anim.SetBool("walking", false);
        }
    }

    private void PlayStepSound()
    {
        if (!myAudioSource.isPlaying && isGrounded)
        {
            myAudioSource.clip = stepSound;
            myAudioSource.pitch = UnityEngine.Random.Range(0.8f, 1.0f);
            myAudioSource.volume = 0.2f;
            myAudioSource.Play();
        }
    }

    public void StretchAndSquash()
    {
        if (!isGrounded && !isWallWalking)
        {
            anim.SetBool("falling", true);
            anim.SetBool("climbing", false);
        }
        else if (anim.GetBool("falling") && isGrounded)
        {
            anim.SetBool("falling", false);
            myAudioSource.clip = landSound;
            myAudioSource.pitch = 1.0f;
            myAudioSource.volume = 1.0f;
            myAudioSource.Play();
            ps.Emit(5);
        }
        else if (isWallWalking && !isGrounded)
        {
            anim.SetBool("climbing", true);
            anim.SetBool("falling", false);
            myAudioSource.clip = landSound;
            myAudioSource.Play();
        }
        else
        {
            anim.SetBool("falling", false);
        }
    }

    public void HaltClimbingAnimation() => anim.speed = 0.0f;
    public void ResumeClimbingAnimation() => anim.speed = 2.0f;
    public void RestartAnimationSpeed() => anim.speed = 1.0f;

    private void DisableAllAnimation()
    {
        GetComponentInChildren<RotationConstraint>().enabled = false;
        anim.enabled = false;
        GetComponentInChildren<PlayerAnimationAndSound>().enabled = false;
    }
    #endregion

    #region Glow FX
    private void EnableEmission(GameObject obj, Material glowMat)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null && glowMat != null)
        {
            renderer.material = glowMat;
            renderer.material.EnableKeyword("_EMISSION");
        }
    }

    private void DisableEmission(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.DisableKeyword("_EMISSION");
            renderer.material.SetColor("_EmissionColor", Color.black);
        }
    }
    #endregion
}

