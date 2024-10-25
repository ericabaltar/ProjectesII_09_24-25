using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public UnityEvent rotationFinishEvent;
    public float rotationSpeed = 90f; // Degrees per second
    public List<GameObject> objectsToConsider; // Assign in the inspector

    private Vector3 centerPoint;

    private void Awake()
    {
        rotationFinishEvent = new UnityEvent();
        rotationFinishEvent.AddListener(RotationEventFinished);
    }

    void Start()
    {

    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            DisableRigidBodiesInScene();
            CalculateCenter();
            StartCoroutine(Move());
        }

    }

    void RotateObjectsInScene()
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
                obj.transform.RotateAround(centerPoint,Vector3.forward, rotationSpeed * Time.deltaTime);
               
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

    IEnumerator Move()
    {
        float totalRotated = 0f;
        while(totalRotated <= 90f)
        {
            totalRotated += rotationSpeed * Time.deltaTime;
            RotateObjectsInScene();
            yield return null; 

        }
        yield return new WaitForFixedUpdate();
        rotationFinishEvent.Invoke();
    }

    void RotationEventFinished()
    {
        EnableRigidBodiesInScene();
    }

}
