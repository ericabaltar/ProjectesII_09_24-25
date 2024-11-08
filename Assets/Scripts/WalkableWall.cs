using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkableWalls : MonoBehaviour
{
    public bool isWalkable = false;
    private Rigidbody2D playerRigidbody;
    [SerializeField] private bool playerIsOnWall = false;
    private float resetGravity;

    public float wallMoveSpeed = 5f; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isWalkable && other.transform.CompareTag("Player"))
        {
            playerRigidbody = other.transform.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                resetGravity = playerRigidbody.gravityScale;
                playerRigidbody.gravityScale = 0;
                playerIsOnWall = true;
                playerRigidbody.velocity = Vector2.zero; // detener cualquier movimiento previo
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isWalkable && other.transform.CompareTag("Player"))
        {
            if (playerRigidbody != null)
            {
                playerRigidbody.gravityScale = resetGravity;
                playerIsOnWall = false;
                playerRigidbody.velocity = Vector2.zero; // detener movimiento al salir de la pared
            }
        }
    }

    private void Update()
    {
        if (playerIsOnWall && playerRigidbody != null)
        {
            Vector2 wallMovement = Vector2.zero;

            
            if (Input.GetKey(KeyCode.W))
            {
                wallMovement = Vector2.up * wallMoveSpeed;
            }
            
            else if (Input.GetKey(KeyCode.S))
            {
                wallMovement = Vector2.down * wallMoveSpeed;
            }

            // si no se presiona ninguna tecla se detiene el movimiento
            playerRigidbody.velocity = wallMovement;
        }
    }
}



