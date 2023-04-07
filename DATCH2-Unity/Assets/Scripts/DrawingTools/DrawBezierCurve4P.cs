using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class DrawBezierCurve4P : MonoBehaviour
{
    public Transform[] points;
    public LineRenderer lineRenderer;
    public int vertexCount = 12;
    public GameObject grabPoint1;
    public GameObject grabPoint4;
    //private GameObject[] endGrabPoints;
    private float closeDistance = 0.1f;
    public bool manualClose = false;


    // Use this for initialization
    void Start () 
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (points == null || points.Length <= 0)
        {
            lineRenderer.positionCount = 0;
            lineRenderer.SetPositions(new Vector3[] { Vector3.zero });
            return;
        }

        var pointList = new List<Vector3>();
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            Vector3 bezierPoint = CalculateBezierPoint(ratio, points.Select(point => point.position));
            pointList.Add(bezierPoint);
        }
        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());

        // Removing code snippet here to add point linking to all points (in PointHelper script)
        //endGrabPoints = GameObject.FindGameObjectsWithTag("EndGrabPoint");

        //// Enables curve linking
        //for (int p = 0; p < endGrabPoints.Length; p++)
        //{

        //    if (Vector3.Distance(grabPoint1.transform.position, endGrabPoints[p].transform.position) < 0.1 && endGrabPoints[p].transform.parent != grabPoint1.transform.parent)
        //    {
        //        grabPoint1.transform.position = endGrabPoints[p].transform.position;

        //    }

        //    if (Vector3.Distance(grabPoint4.transform.position, endGrabPoints[p].transform.position) < 0.1 && endGrabPoints[p].transform.parent != grabPoint1.transform.parent)
        //    {
        //        grabPoint4.transform.position = endGrabPoints[p].transform.position;
        //    }

        //}

        
        // Closes shape at certain distance
        if (Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(lineRenderer.positionCount - 1)) <= closeDistance)
        {
            lineRenderer.loop = true;
        }

        if (Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(lineRenderer.positionCount - 1)) > closeDistance)
        {
            lineRenderer.loop = false;
        }

        if (manualClose == true)
        {
            lineRenderer.loop = true;
        }
    }

    private Vector3 CalculateBezierPoint(float ratio, IEnumerable<Vector3> points)
    {
        if (points.Count() == 1)
        {
            return points.First();
        }

        LinkedList<Vector3> subPoints = new LinkedList<Vector3>();
        Vector3? lastPoint = null;
        foreach (var point in points)
        {
            if (!lastPoint.HasValue)
            {
                lastPoint = point;
                continue;
            }
            else
            {
                subPoints.AddLast(Vector3.Lerp(lastPoint.Value, point, ratio));

                lastPoint = point;
            }
        }

        return CalculateBezierPoint(ratio, subPoints);
    }
}