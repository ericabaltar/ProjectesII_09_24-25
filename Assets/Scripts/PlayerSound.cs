using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private AudioSource audioSource;
    private Rigidbody2D rb;

    public Vector2 velocity;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.velocity.y < -4)
        {
            audioSource.Play();
            Debug.Log("SONUIDO");
        }
    }

}
