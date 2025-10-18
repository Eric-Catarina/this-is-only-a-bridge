using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class RopeVisualizer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<Transform> anchorPoints;

    public void Initialize(List<Transform> points, Material material, float width)
    {
        lineRenderer = GetComponent<LineRenderer>();
        anchorPoints = points.Where(p => p != null).ToList();

        lineRenderer.material = material;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = anchorPoints.Count;
    }

    private void LateUpdate()
    {
        if (anchorPoints == null || anchorPoints.Count == 0) return;

        for (int i = 0; i < anchorPoints.Count; i++)
        {
            if (anchorPoints[i] != null)
            {
                lineRenderer.SetPosition(i, anchorPoints[i].position);
            }
        }
    }
}