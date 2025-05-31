using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float movementSpeed = 3f;
    private Rigidbody2D characterRigidBody;

    private ControllerInputSystem inputSystem;
    private Vector2 inputDirection;

    [Header("Particles")]
    public LayerMask layerMask;
    public ParticleSystem particlesLeft;
    public ParticleSystem particlesRight;
    public ParticleSystem particlesUp;
    public ParticleSystem particlesDown;

    [Header("UI & Scene")]
    public List<MoveUiToCenter> moveUiToCenterList = new List<MoveUiToCenter>();
    public AudioSource sceneSound;

    private Scene scene;
    private float transitionDelay = 2f;

    public bool isWallWalking { get; private set; } = false;

    private void Awake()
    {
        characterRigidBody = GetComponent<Rigidbody2D>();
        inputSystem = GetComponent<ControllerInputSystem>();

        if (inputSystem == null)
        {
            Debug.LogError("ControllerInputSystem no encontrado.");
        }
    }

    private void OnEnable()
    {
        if (inputSystem != null)
        {
            inputSystem.enabled = true;
        }
    }

    private void OnDisable()
    {
        if (inputSystem != null)
        {
            inputSystem.enabled = false;
        }
    }

    private void Update()
    {
        if (inputSystem != null)
        {
            inputDirection = inputSystem.MovementValue;
        }
    }

    private void FixedUpdate()
    {
        Vector2 movement = inputDirection.normalized;
        characterRigidBody.velocity = movement * movementSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Door"))
        {
            if (sceneSound != null)
            {
                sceneSound.Play();
                foreach (var ui in moveUiToCenterList)
                {
                    ui.MoveToCloseCurtains();
                }
            }
            StartCoroutine(WaitAndLoadScene(transitionDelay));
        }
    }

    private IEnumerator WaitAndLoadScene(float time)
    {
        yield return new WaitForSeconds(time);
        scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex + 1);
    }
}
