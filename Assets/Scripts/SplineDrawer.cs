using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SplineDrawer : MonoBehaviour
{
    public SplineContainer splineContainer; // Reference to your spline
    private LineRenderer lineRenderer;
    private Spline spline;
    private Transform parent;
    bool hasBeenDone = false;
    // Start is called before the first frame update
    void Awake()
    {
        
        lineRenderer = GetComponent<LineRenderer>();
        parent = GetComponentInParent<Transform>();
        // Get the first spline in the container
        if (splineContainer.Splines.Count > 0)
        {
            spline = splineContainer.Splines[0];
            UpdateLineRenderer(); // Draw the existing spline
        }
        else
        {
            Debug.LogWarning("No splines found in SplineContainer!");
        }
    }

    private void Update()
    {
        
        UpdateLineRenderer();
        
    }

    void AddPoint(Vector3 position)
    {
        position = splineContainer.transform.InverseTransformPoint(position);
        spline.Add(new BezierKnot(position));
        UpdateLineRenderer();
    }


    void UpdateSpline()
    {


        if (GameManager.Instance.rotationState == GameManager.RotationState.IDLE && !hasBeenDone)
        {
            Quaternion parentRot = parent.rotation;
            for (int i = 0; i < splineContainer.Spline.Count; i++)
            {
                BezierKnot knot = splineContainer.Spline[i];
                // Convert the knot's local position to world space
                Vector3 worldPos = splineContainer.transform.TransformPoint(knot.Position);
                // Calculate the offset from the parent's position
                Vector3 offset = worldPos - parent.position;
                // Rotate the offset by the parent's rotation
                Vector3 rotatedOffset = parentRot * offset;
                // Compute the new world position
                Vector3 newWorldPos = parent.position + rotatedOffset;
                // Convert back to the splineContainer's local space
                knot.Position = splineContainer.transform.InverseTransformPoint(newWorldPos);
                splineContainer.Spline[i] = knot;
                
            }
            hasBeenDone = true;
        }
        else if (GameManager.Instance.rotationState != GameManager.RotationState.IDLE && hasBeenDone)
        {
            hasBeenDone = false;
        }




    }

    void UpdateLineRenderer()
    {
        int pointCount = spline.Count;
        if (pointCount == 0)
        {
            Debug.LogWarning("Spline has no points!");
            return;
        }

        lineRenderer.positionCount = pointCount;

        for (int i = 0; i < pointCount; i++)
        {
            Vector3 worldPos = splineContainer.transform.TransformPoint(spline[i].Position);
            lineRenderer.SetPosition(i, worldPos);
        }
    }
}
