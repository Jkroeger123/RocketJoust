using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer)), ExecuteAlways]
public class DrawBezier : MonoBehaviour
{
    public Transform pointOne;
    public Transform pointTwo;
    public Transform pointThree;
        
    [Range(2, 50)]
    public int resolution = 10;

    public float zPosition = 1f;
    
    private LineRenderer _lineRenderer;
    private Vector3[] _points;
    
    private void Awake()
    {
        _points = new Vector3[resolution];
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = resolution;
    }
    
    private void Update()
    {
        UpdateLineRenderer();
    }

    private void UpdateLineRenderer()
    {
        for (int i = 1; i < resolution+1; i++)
        {
            float t = i / (float) resolution;
            _points[i - 1] = CalculateBezierPoint(t, pointOne.position, pointTwo.position, pointThree.position);
            _points[i - 1].z = zPosition;
        }
        _lineRenderer.SetPositions(_points);
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }

}
