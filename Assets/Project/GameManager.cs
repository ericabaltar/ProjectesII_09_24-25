using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public UnityEvent rotationFinishEvent;
    public float rotationSpeed = 90.0f; // Degrees per second
    public List<GameObject> objectsToConsider; // Assign in the inspector
    public PlayerController playerController;
    private Rigidbody2D rb;
    private Vector3 centerPoint;
    private float threshold = 0.1f;
    public AudioSource rotationSound;

    public bool isRotating = false;

    private void OnEnable()
    {
        if (Instance != null)
            return;

        Instance = this;
    }

    private void Awake()
    {
        rotationFinishEvent = new UnityEvent();
        rotationFinishEvent.AddListener(RotationEventFinished);
        rb = playerController.GetComponent<Rigidbody2D>();    
    }

    void Update()
    {
        if(!isRotating && (Mathf.Abs(rb.velocity.x) < threshold && (Mathf.Abs(rb.velocity.y) < threshold)))
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                isRotating = true;
                DisableRigidBodiesInScene();
                CalculateCenter();
                StartCoroutine(Move(rotationSpeed));
                PlayRotationSound();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                isRotating = true;
                DisableRigidBodiesInScene();
                CalculateCenter();
                StartCoroutine(Move(-rotationSpeed));
                PlayRotationSound();
            }
        }
       

    }

    void RotateObjectsInScene(float speedRotation)
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
                obj.transform.RotateAround(centerPoint,Vector3.forward, speedRotation * Time.deltaTime);
               
            }
        }
    }

    void DisableRigidBodiesInScene()
    {
        //Rigidbody2D rb;
        foreach (GameObject obj in objectsToConsider)
        {
            if (obj != null)
            {
                if (obj.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.simulated = false;
                }
            }
        }
    }
    void EnableRigidBodiesInScene()
    {
        //Rigidbody2D rb;
        foreach (GameObject obj in objectsToConsider)
        {
            if (obj != null)
            {
                if (obj.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.simulated = true;
                }
            }

            Vector3 currentEulerAngles = obj.transform.eulerAngles;

            float newValueZ = Mathf.RoundToInt(currentEulerAngles.z);


            obj.transform.rotation =Quaternion.Euler(obj.transform.eulerAngles.x, obj.transform.eulerAngles.y,newValueZ );
            Debug.Log($"X: {currentEulerAngles.x}, Y: {currentEulerAngles.y}, Z: {newValueZ}");

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
        /*
        foreach (GameObject obj in objectsToConsider)
        {
            if (obj != null)
            {
                sum += obj.transform.position;
            }
        }
        
        centerPoint = sum / objectsToConsider.Count;
        Debug.Log("Center Point: " + centerPoint);
        */
    }

    IEnumerator Move(float speedRotation)
    {
        float totalRotated = 0f;
        while(totalRotated <= 90.00f)
        {
            float frameThisRotation = rotationSpeed * Time.deltaTime;
            totalRotated += frameThisRotation;
            RotateObjectsInScene(speedRotation);
            yield return null; 

        }
        
        yield return new WaitForFixedUpdate();
        rotationFinishEvent.Invoke();
        isRotating = false;
    }

    void RotationEventFinished()
    {
        EnableRigidBodiesInScene();
    }


    public bool GetIsRotating()
    {
        return isRotating;
    }

    void PlayRotationSound()
    {
        if (rotationSound != null)
        {
            rotationSound.Play();
        }
        else
        {
            Debug.LogWarning("No se ha asignado un sonido.");
        }
    }




}
