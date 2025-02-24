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

    public float[] PossibleRotations = {90, 180,270,360,0 };
    private int targetRotation = 0; //index of PossibleRotations

    public const float RotationAngle = 90.0f; //Deg
    public const float RotationSpeed = 90.0f; //Deg/s
    private float lastAngle = 0.0f;

    public AnimationCurve rotationCurve;
    private float rotationTime = 0.0f;
    private float currentRotationTime = 0.0f;
    private float remainingRotation = 0.0f;

    public List<GameObject> objectsToConsider; // Assign in the inspector
    public List<GameObject> objectsToConsiderWorld2; // Assign in the inspector
    public List<GameObject> objectsToConsiderWorld3; // Assign in the inspector
    public List<GameObject> objectsToConsiderWorld4; // Assign in the inspector
    private Vector3 centerPoint;
    private Vector3 centerPoint2;
    private Vector3 centerPoint3;
    private Vector3 centerPoint4;

    public AudioSource rotationSound;
    private float cumulativeRotation = 0.0f;

    public bool isRotating { get; private set; } = false;
    private bool rotatingRight = false;

    [field:Header("Bool Choose Middle Point")]
    public enum TypeOfCenter { None,MiddlePoint, GameObject, Worlds};
    public TypeOfCenter typeOfCenter;
    [SerializeField, HideInInspector] private GameObject centerGameObject;


    public TypeOfCenter GetTypeOfCenter()
    {
        return typeOfCenter;
    }



    private void OnEnable()
    {
        if (Instance != null)
            return;

        Instance = this;

        if (typeOfCenter == TypeOfCenter.Worlds)
        {
            // PASS IT FOR EVERY LIST
            centerPoint = CalculateCenterOfList(objectsToConsider);
            centerPoint2 = CalculateCenterOfList(objectsToConsiderWorld2);
            centerPoint3 = CalculateCenterOfList(objectsToConsiderWorld3);
            centerPoint4 = CalculateCenterOfList(objectsToConsiderWorld4);
        }
    }

    private void RotatingStateUpdate()
    {
        float fixedAnglePerFrame = 90f;
        
        float rotationAngle = rotatingRight ? -fixedAnglePerFrame : fixedAnglePerFrame;
        rotationAngle *= Time.deltaTime;
        RotateObjectsInScene(rotationAngle);
        cumulativeRotation += rotationAngle;
        if (Mathf.Abs(cumulativeRotation) >= RotationAngle)
        {
            StopRotation(); // Automatically stop rotation
        }


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

        if (t >= 1.0f)
        {
            StartCoroutine(WaitFixedUpdateAndEnableRigidbodies());
            rotationFinishEvent.Invoke();
            rotationState = RotationState.IDLE;
        }
    }

    void Update()
    {
        if(typeOfCenter == TypeOfCenter.MiddlePoint)
        {
            CalculateCenter();
        }
        else if(typeOfCenter == TypeOfCenter.None)
        {
            centerPoint = Vector3.zero;
        }
        else if (typeOfCenter == TypeOfCenter.GameObject)
        {
            centerPoint = centerGameObject.transform.position;
        }
        

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
        PlayRotationSound();
        rotatingRight = goesRight;
        lastAngle = 0;
        currentRotationTime = 0.0f;
        cumulativeRotation = 0.0f;
        ToggleRigidbodiesInScene(false);
    }

    public void StopRotation()
    {
        if (rotationState != RotationState.ROTATING)
            return;

        float currentAngle = NormalizeAngle(objectsToConsider[0].transform.eulerAngles.z);

        
        targetRotation = 0;
        remainingRotation = Mathf.Infinity;

        // Find the closest valid rotation in PossibleRotations
        for (int i = 0; i < PossibleRotations.Length; i++)
        {
            float validAngle = PossibleRotations[i];
            float dist = Mathf.Abs((360 + validAngle) - (360 + currentAngle)); 

            if (dist < remainingRotation)
            {
                rotatingRight = (validAngle) - (currentAngle) < 0;
                targetRotation = i;
                remainingRotation = dist;
            }
        }


        rotationState = RotationState.ADJUSTING;

        float breakingFactor = 1f; 
        rotationTime = (remainingRotation * breakingFactor) / RotationSpeed;
        currentRotationTime = 0.0f;
    }

    void RotateObjectsInScene(float angle)
    {
        
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

        foreach (GameObject obj in objectsToConsiderWorld2)
        {
            if (obj != null && obj.tag != "StaticText")
            {
                if (obj.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.simulated = false;
                }
                // Rotate around the Y-axis
                obj.transform.RotateAround(centerPoint2, Vector3.forward, angle);

            }
        }

        foreach (GameObject obj in objectsToConsiderWorld3)
        {
            if (obj != null && obj.tag != "StaticText")
            {
                if (obj.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.simulated = false;
                }
                // Rotate around the Y-axis
                obj.transform.RotateAround(centerPoint3, Vector3.forward, angle);

            }
        }

        foreach (GameObject obj in objectsToConsiderWorld4)
        {
            if (obj != null && obj.tag != "StaticText")
            {
                if (obj.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.simulated = false;
                }
                // Rotate around the Y-axis
                obj.transform.RotateAround(centerPoint4, Vector3.forward, angle);

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

        foreach (GameObject obj in objectsToConsiderWorld2)
        {
            if (obj != null)
            {
                if (obj.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.simulated = active;
                }
            }
        }

        foreach (GameObject obj in objectsToConsiderWorld3)
        {
            if (obj != null)
            {
                if (obj.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.simulated = active;
                }
            }
        }

        foreach (GameObject obj in objectsToConsiderWorld4)
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

        centerPoint = Camera.main.transform.position;
    }

    Vector3 CalculateCenterOfList(List<GameObject> worldList)
    {
        if (worldList == null || worldList.Count == 0)
            return Vector3.zero;

        Vector3 sum = Vector3.zero;

        foreach (GameObject obj in worldList)
        {
            if (!obj.TryGetComponent(out Rigidbody2D rb))
                sum += transform.InverseTransformPoint(obj.transform.position);
        }

        return sum / worldList.Count; // Promedio de posiciones
        
    }

    void OnDrawGizmos()
    {
        if (objectsToConsider == null || objectsToConsider.Count == 0)
            return;

        Gizmos.color = Color.red; // Color del punto
        Gizmos.DrawSphere(centerPoint, 0.2f); // Dibuja una esfera en el centro
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

    private float NormalizeAngle(float angle)
    {
        return (angle % 360f + 360f) % 360f;
    }

    
}
