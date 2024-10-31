using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkableWalls : MonoBehaviour
{
    public bool isWalkable = false;
    private Rigidbody playerRigidbody; 
    private bool playerIsOnWall = false;



    private void OnTriggerEnter(Collider other)
    {

        if (isWalkable && other.CompareTag("Player"))
        {
            
            playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                playerRigidbody.useGravity = false; 
                playerIsOnWall = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isWalkable && other.CompareTag("Player"))
        {
            
            if (playerRigidbody != null)
            {
                playerRigidbody.useGravity = true; 
                playerIsOnWall = false;
            }
        }
    }

    private void Update()
    {
        if (playerIsOnWall && playerRigidbody != null)
        {
            
            playerRigidbody.velocity = new Vector3(0, 0, 0); 

            
            if (Input.GetKey(KeyCode.W))
            {
                playerRigidbody.position += transform.up * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                playerRigidbody.position -= transform.up * Time.deltaTime;
            }
        }
    }
}


