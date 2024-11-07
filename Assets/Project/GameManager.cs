using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public UnityEvent rotationFinishEvent;
    public float rotationSpeed = 90.0f; // Degrees per second
    public List<GameObject> objectsToConsider; // Assign in the inspector

    private Vector3 centerPoint;

    public bool isRotating = false;

    private void Awake()
    {
        rotationFinishEvent = new UnityEvent();
        rotationFinishEvent.AddListener(RotationEventFinished);
    }

    void Update()
    {
        if(!isRotating)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                isRotating = true;
                DisableRigidBodiesInScene();
                CalculateCenter();
                StartCoroutine(Move(rotationSpeed));
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                isRotating = true;
                DisableRigidBodiesInScene();
                CalculateCenter();
                StartCoroutine(Move(-rotationSpeed));
            }
        }
       

    }

    void RotateObjectsInScene(float speedRotation)
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

    


}
