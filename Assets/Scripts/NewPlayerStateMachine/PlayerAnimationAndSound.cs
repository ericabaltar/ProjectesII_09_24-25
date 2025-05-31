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





    void Start()
    {
        ConstraintSource source = new ConstraintSource
        {
            sourceTransform = Camera.main.transform,
            weight = 1.0f
        };
        GetComponent<RotationConstraint>().SetSource(0, source);
    }



}

