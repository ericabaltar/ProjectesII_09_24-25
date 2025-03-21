using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    Vector3 playerStartPos;
    Vector3 cameraStartPos;
    
    Vector3 playerDistance;

    public PlayerStateMachine ps;
    [SerializeField] public float force;

    void Start()
    {
        cameraStartPos = transform.position;
        playerStartPos = ps.GetComponent<Transform>().position;

    }

    void Update()
    {
        playerDistance = ps.GetComponent<Transform>().position - playerStartPos;

        transform.position = cameraStartPos + (playerDistance / 10.0f) * force;
    }
}
