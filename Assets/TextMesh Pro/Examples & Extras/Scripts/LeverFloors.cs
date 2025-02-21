using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LeverFloors : MonoBehaviour
{
    bool playerTouch = false;
    bool flipped = false;
    public GameObject platformHolder;
    public Tilemap tiles;
    public Sprite rightSp;
    public Sprite leftSp;

    public Color col;

    private void Update()
    {   
        if (playerTouch && Input.GetKeyDown(KeyCode.Space))
        {
            flipped = !flipped;

            if (flipped)
                gameObject.GetComponent<SpriteRenderer>().sprite = leftSp;
            else
                gameObject.GetComponent<SpriteRenderer>().sprite = rightSp;


            for (int i = 0; i < platformHolder.gameObject.transform.childCount; i++)
            {
                GameObject child = platformHolder.gameObject.transform.GetChild(i).gameObject;

                if (child.GetComponent<BoxCollider2D>().enabled == false)
                {                    

                    child.GetComponent<BoxCollider2D>().enabled = true;

                    tiles.color = new Color(
                            col.r
                            , col.g
                            , col.b
                            , 1.0f);
                }
                else
                {                    
                    

                    child.GetComponent<BoxCollider2D>().enabled = false;

                    tiles.color = new Color(
                            col.r
                            , col.g
                            , col.b
                            , 20.0f / 255.0f);
                }
            }            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            playerTouch = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            playerTouch = false;
    }        
}
