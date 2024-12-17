using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI.Table;

public class GameManager : MonoBehaviour
{
    public enum RotationState { IDLE, ROTATING, ADJUSTING }

    public RotationState rotationState { get; private set; } = RotationState.IDLE;

    public static GameManager Instance;

    public UnityEvent rotationFinishEvent;

    public float[] PossibleRotations = {90, 180,270,360 };
    private int targetRotation = 0; //index of PossibleRotations

    public const float RotationAngle = 90.0f; //Deg
    public const float RotationSpeed = 90.0f; //Deg/s
    private float lastAngle = 0.0f;

    public AnimationCurve rotationCurve;
    private float rotationTime = 0.0f;
    private float currentRotationTime = 0.0f;
    private float remainingRotation = 0.0f;

    public List<GameObject> objectsToConsider; // Assign in the inspector
    private Vector3 centerPoint;
    public AudioSource rotationSound;

    public bool isRotating { get; private set; } = false;
    private bool rotatingRight = false;

    private void OnEnable()
    {
        if (Instance != null)
            return;

        Instance = this;
    }

    private void RotatingStateUpdate()
    {
        float rotationAngle = lastAngle + Time.deltaTime;
        lastAngle = rotationAngle;
        RotateObjectsInScene(rotationAngle);
        //If key is released stop rotation
        //If degrees are fine stop rotation
    }

    private void AdjustingStateUpdate()
    {
        currentRotationTime = Mathf.Min(currentRotationTime + Time.deltaTime, rotationTime);
        float t = currentRotationTime / rotationTime;

        float rot = rotationCurve.Evaluate(t) * (rotatingRight ? -1.0f : 1.0f);
        rot *= remainingRotation;

        //update rotation
        float rotationAngle = rot - lastAngle;
        lastAngle = rot;
        RotateObjectsInScene(rotationAngle);

        if (t == 1.0f)
        {
            StopRotation();
            //Rotation completed
            StartCoroutine(WaitFixedUpdateAndEnableRigidbodies());
            rotationFinishEvent.Invoke();
            rotationState = RotationState.IDLE;
        }
    }
    void Update()
    {
        switch (rotationState)
        {
            case RotationState.ROTATING:
                RotatingStateUpdate();
                break;
            case RotationState.ADJUSTING:
                AdjustingStateUpdate();
                break;
        }
    }

    public void StartRotation(bool goesRight)
    {
        if (rotationState != RotationState.IDLE)
            return;

        rotationState = RotationState.ROTATING;
        rotatingRight = goesRight;
        PlayRotationSound();
        lastAngle = 0;
        currentRotationTime = 0.0f;
        ToggleRigidbodiesInScene(false);
    }

    public void StopRotation()
    {
        if (rotationState != RotationState.ROTATING)
            return;

        //Calculate the closest valid angle
        targetRotation = 0;
        float currentAngle = transform.rotation.eulerAngles.z;
        if (float.IsNaN(currentAngle))
        {
            Debug.LogError("Current angle is NaN. Ensure transform.rotation is valid.");
            return;
        }
        remainingRotation = (360f + PossibleRotations[targetRotation]) - (360f + currentAngle);
        for(int i = 1; i < PossibleRotations.Length; i++) {
            float dist = (360f + PossibleRotations[i]) - (360f + currentAngle);
            if (dist < remainingRotation)
            {
                targetRotation = i;
                remainingRotation = dist;
            }
        }

        rotationState = RotationState.ADJUSTING;

        float breakingFactor = 1f; //Smooths the ending
        rotationTime = (remainingRotation * breakingFactor) / RotationSpeed;
        currentRotationTime = 0.0f;
    }

    void RotateObjectsInScene(float angle)
    {
        //Rigidbody2D rb;
        foreach (GameObject obj in objectsToConsider)
        {
            if (obj != null && obj.tag != "StaticText")
            {
                if (obj.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.simulated = false;
                }
                // Rotate around the Y-axis
                obj.transform.RotateAround(centerPoint,Vector3.forward, angle);
               
            }
        }
    }

    void ToggleRigidbodiesInScene(bool active)
    {
        //Rigidbody2D rb;
        foreach (GameObject obj in objectsToConsider)
        {
            if (obj != null)
            {
                if (obj.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.simulated = active;
                }
            }
        }
    }

    void CalculateCenter()
    {
        if (objectsToConsider.Count == 0)
        {
            Debug.LogWarning("No objects assigned to calculate center.");
            return;
        }

        Vector3 sum = Vector3.zero;
        centerPoint = Camera.main.transform.position;
    }

    IEnumerator WaitFixedUpdateAndEnableRigidbodies()
    {
        yield return new WaitForFixedUpdate();
        ToggleRigidbodiesInScene(true);
    }

    void PlayRotationSound()
    {
        if (rotationSound != null)
            rotationSound.Play();
        else
            Debug.LogWarning("No se ha asignado un sonido.");
    }


    public void SetRotationState(RotationState newState)
    {
        rotationState = newState;
    }



}
