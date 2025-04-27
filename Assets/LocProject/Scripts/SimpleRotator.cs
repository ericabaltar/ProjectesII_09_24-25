using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    public float degreesPerSecond = 30.0f;

    // Update is called once per frame
    void Update()
    {
        this.transform.localRotation *= Quaternion.Euler(0, 0, degreesPerSecond * Time.deltaTime);
    }
}
