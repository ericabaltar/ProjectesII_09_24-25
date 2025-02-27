using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
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

    enum TypeOfLeaver {WallSwitcher, Platforms }
    [SerializeField] TypeOfLeaver useOfLeaver = TypeOfLeaver.WallSwitcher;

    //Spline
    public SplineContainer splineContainer; // Assign the SplineContainer in Inspector
    public float speed = 2f; // Speed of movement along the spline
    private float progress = 0f; // 0 (start) to 1 (end)
    private bool movingForward = false; // True = down, False = up

    private void Update()
    {   
        switch(useOfLeaver)
        {
            case TypeOfLeaver.WallSwitcher:
                UpdateWallSwitcher();
                break;
            case TypeOfLeaver.Platforms:
                UpdateMovingPlatform();
                break;
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

    void PlayLever() {
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.clip = Resources.Load<AudioClip>("lever");
        newSource.PlayOneShot(newSource.clip);

        Destroy(newSource, newSource.clip.length);
    }
    void PlayWallAppear() {
        AudioSource newSource2 = gameObject.AddComponent<AudioSource>();
        newSource2.clip = Resources.Load<AudioClip>("wallAppearSound");
        newSource2.PlayOneShot(newSource2.clip);
        
        Destroy(newSource2, newSource2.clip.length);
    }
    void PlayWallDissappear() {
        AudioSource newSource3 = gameObject.AddComponent<AudioSource>();
        newSource3.clip = Resources.Load<AudioClip>("wallDissappearSound");
        newSource3.PlayOneShot(newSource3.clip);

        Destroy(newSource3, newSource3.clip.length);
    }


    void UpdateWallSwitcher()
    {
        if (playerTouch && Input.GetKeyDown(KeyCode.Space))
        {
            flipped = !flipped;

            if (flipped)
                gameObject.GetComponent<SpriteRenderer>().sprite = leftSp;
            else
                gameObject.GetComponent<SpriteRenderer>().sprite = rightSp;

            PlayLever();

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

                    PlayWallAppear();
                }
                else
                {


                    child.GetComponent<BoxCollider2D>().enabled = false;

                    tiles.color = new Color(
                            col.r
                            , col.g
                            , col.b
                            , 20.0f / 255.0f);

                    PlayWallDissappear();
                }
            }
        }
    }


    void UpdateMovingPlatform()
    {

        if (playerTouch && Input.GetKeyDown(KeyCode.Space))
        {
            flipped = !flipped;
            movingForward = !movingForward;

            if (flipped)
                gameObject.GetComponent<SpriteRenderer>().sprite = leftSp;
            else
                gameObject.GetComponent<SpriteRenderer>().sprite = rightSp;

            PlayLever();

            /////////
            
        }

        if (splineContainer == null || splineContainer.Splines.Count == 0)
            return;

        // Get the first spline
        Spline spline = splineContainer.Splines[0];

        // Move along the spline
        if (movingForward && progress < 1f)
        {
            progress += speed * Time.deltaTime / spline.GetLength();
        }
        else if (!movingForward && progress > 0f)
        {
            progress -= speed * Time.deltaTime / spline.GetLength();
        }

        // Clamp progress between 0 and 1
        progress = Mathf.Clamp01(progress);

        // Set the cube position along the spline
        transform.position = spline.EvaluatePosition(progress);


    }
}
