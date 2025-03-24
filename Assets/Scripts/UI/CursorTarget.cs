using UnityEngine;

public class CursorFollow : MonoBehaviour
{
    public Camera mainCamera;
    public float followSpeed = 5f;  // Smoothness

    void Update()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Keep Z consistent for 2D
        transform.position = Vector3.Lerp(transform.position, mousePos, followSpeed * Time.deltaTime);
    }
}

