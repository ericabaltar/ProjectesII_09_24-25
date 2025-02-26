using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineDrawer : MonoBehaviour
{
    public SplineContainer splineContainer; // Reference to your spline
    private LineRenderer lineRenderer;
    private Spline spline;

    // Start is called before the first frame update
    void Awake()
    {
        splineContainer = GetComponent<SplineContainer>();
        lineRenderer = GetComponent<LineRenderer>();

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

    void AddPoint(Vector3 position)
    {
        position = splineContainer.transform.InverseTransformPoint(position);
        spline.Add(new BezierKnot(position));
        UpdateLineRenderer();
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
