using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStateMachine : StateMachine
{

    [field: SerializeField] public Rigidbody2D rigidbody2d;
    [field: SerializeField] public ControllerInputSystem InputReader { get; private set; }

    [field: SerializeField] public float playerSpeed = 100f;
    public bool isGrounded { get; private set; } = true;
    public bool isWallWalking { get; private set; } = false;
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
        if ((isGrounded || isWallWalking) && (rigidbody2d.velocity.magnitude < 0f || rigidbody2d.velocity.magnitude > 0f))
        {
            GameManager.Instance.StartRotation(false);
        }
       
    }

    public void RotateRight()
    {
        if ((isGrounded || isWallWalking) && (rigidbody2d.velocity.magnitude < 0.0f || rigidbody2d.velocity.magnitude > 0.0f))
        {
            GameManager.Instance.StartRotation(true);
        }
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.transform.CompareTag("Untagged") || collision.transform.CompareTag("Walkable"))
        {
            isGrounded = true;
        }

        if (collision.transform.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
