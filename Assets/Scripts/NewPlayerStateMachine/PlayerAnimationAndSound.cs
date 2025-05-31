using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerAnimationAndSound : MonoBehaviour
{
    private Animator anim;
    private bool canRotate = true;
    private bool canStretch = true;
    private bool canSquish = false;

    private PlayerStateMachine playerState;
    private SpriteRenderer mySprite;
    private AudioSource myAudioSource;

    [field: SerializeField] public AudioClip landSound;
    [field: SerializeField] public AudioClip stepSound;

    private InputAction nextSceneAction;
    private InputAction prevSceneAction;

    private void Awake()
    {
        
        nextSceneAction = new InputAction(binding: "<Keyboard>/0");
        prevSceneAction = new InputAction(binding: "<Keyboard>/9");

        nextSceneAction.performed += _ => LoadNextScene();
        prevSceneAction.performed += _ => LoadPreviousScene();
    }

    private void OnEnable()
    {
        nextSceneAction.Enable();
        prevSceneAction.Enable();
    }

    private void OnDisable()
    {
        nextSceneAction.Disable();
        prevSceneAction.Disable();
    }

    void Start()
    {
        ConstraintSource source = new ConstraintSource
        {
            sourceTransform = Camera.main.transform,
            weight = 1.0f
        };
        GetComponent<RotationConstraint>().SetSource(0, source);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void LoadPreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}

