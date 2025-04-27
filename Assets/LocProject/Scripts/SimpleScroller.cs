using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SimpleScroller : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    [SerializeField] private Vector2 scrollFactor;
    // Update is called once per frame
    void Update()
    {
        rawImage.uvRect = new Rect(rawImage.uvRect.position + scrollFactor * Time.deltaTime, rawImage.uvRect.size);
    }
}
