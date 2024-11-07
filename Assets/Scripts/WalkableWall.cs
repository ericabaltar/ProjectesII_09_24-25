using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkableWalls : MonoBehaviour
{
    public bool isWalkable = false;
    private Rigidbody2D playerRigidbody; 
    [SerializeField]private bool playerIsOnWall = false;
    private float resetGravity;


    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (isWalkable && other.transform.CompareTag("Player"))
        {
            
            playerRigidbody = other.transform.GetComponent<Rigidbody2D>();
            Debug.Log(playerRigidbody);
            if (playerRigidbody != null)
            {
                resetGravity = playerRigidbody.gravityScale;
                playerRigidbody.gravityScale = 0; 
                playerIsOnWall = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (isWalkable && other.transform.CompareTag("Player"))
        {
            
            if (playerRigidbody != null)
            {
                playerRigidbody.gravityScale = resetGravity; 
                playerIsOnWall = false;
            }
        }
    }

    private void Update()
    {
        if (playerIsOnWall && playerRigidbody != null)
        {
            
            //playerRigidbody.velocity = new Vector3(0, 0, 0); 

            
            if (Input.GetKey(KeyCode.W))
            {
                playerRigidbody.AddForce(transform.up);
            }
            if (Input.GetKey(KeyCode.S))
            {
                playerRigidbody.AddForce(-transform.up);
            }
        }
    }
}


