using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public UnityEvent rotationFinishEvent;

    public AnimationCurve rotationCurve;
    public const float RotationAngle = 90.0f;
    public float rotationTime = 2.0f;
    private float currentRotationTime = 0.0f;
    private float lastAngle = 0.0f;

    public List<GameObject> objectsToConsider; // Assign in the inspector
    public PlayerController playerController;
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
    void Update()
    {
        if (isRotating)
        { //Update rotation
            currentRotationTime = Mathf.Min(currentRotationTime + Time.deltaTime, rotationTime);
            float t = currentRotationTime / rotationTime;

            float rot = rotationCurve.Evaluate(t) * (rotatingRight ? -1.0f : 1.0f);
            rot *= RotationAngle;

            //update rotation
            float rotationAngle = rot - lastAngle;
            lastAngle = rot;
            RotateObjectsInScene(rotationAngle);

            if (t == 1.0f)
            {
                //Rotation completed
                StartCoroutine(WaitFixedUpdateAndEnableRigidbodies());
                rotationFinishEvent.Invoke();
                isRotating = false;
            }
        }
    }

    public void StartRotation(bool goesRight)
    {
        isRotating = true;
        rotatingRight = goesRight;
        PlayRotationSound();
        lastAngle = 0;
        currentRotationTime = 0.0f;
        ToggleRigidbodiesInScene(false);
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
}
