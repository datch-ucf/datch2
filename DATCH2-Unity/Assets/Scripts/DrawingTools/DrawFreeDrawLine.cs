using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;

public class DrawFreeDrawLine : MonoBehaviour
{
    public LineRenderer lineRendererOrigin;
    public GameObject freeDrawPointEmpty;
    public List<Vector3> pointList = new List<Vector3>();
    public List<Transform> freeDrawPointEmptyTransforms = new List<Transform>();
    private LineRenderer newLineRenderer;
    public GameObject shape;
    private GameObject gestureContainer;
    public GameObject newFreeDrawPointEmpty;
    private Vector3 previousPosition;
    private Vector3 currentPosition;
    private float closeDistance = 0.1f;
    private bool firstPointCreated = false;
    public bool manualClose = false;

    void Start()
    {
        gestureContainer = Camera.main.transform.GetChild(0).gameObject;
        newLineRenderer = Instantiate(lineRendererOrigin, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        newLineRenderer.useWorldSpace = true; // need to set to true for correct behavior on move, rotate, or scale
        shape = Instantiate(shape, gameObject.transform.position, Quaternion.identity);
        newLineRenderer.transform.parent = shape.transform;
        gameObject.transform.parent = shape.transform;
        currentPosition = gameObject.transform.position;
        AddPointToLine();
    }
    void Update()
    {
        currentPosition = gameObject.transform.position;

        // if point has moved, add a point to the LineRenderer
        if (currentPosition != previousPosition)
        {
            AddPointToLine();
        }

        // Controls shape looping (for closing shapes)
        for (int i = 0; i < (pointList.ToArray()).Length; i++)
        {
            newLineRenderer.SetPosition(i, freeDrawPointEmptyTransforms[i].position);

            // Looping
            // if current point is very close to first point and the line is a substantial length, allow closing to occur
            if (Vector3.Distance(freeDrawPointEmptyTransforms[i].position, freeDrawPointEmptyTransforms[0].position) < closeDistance && newLineRenderer.positionCount > 50) 
            {
                newLineRenderer.loop = true;
            }
            // Not looping
            else
            {
                newLineRenderer.loop = false;
            }
        }

        if(manualClose == true)
        {
            newLineRenderer.loop = true;
        }
    }

    public void AddPointToLine()
    {
        // Prevents extra first freeDrawPointEmpty from being created
        if (firstPointCreated == false)
        {
            newFreeDrawPointEmpty = gameObject;
            firstPointCreated = true;
        }
        else
        {
            newFreeDrawPointEmpty = Instantiate(freeDrawPointEmpty, currentPosition, Quaternion.identity);
            newFreeDrawPointEmpty.transform.parent = shape.transform;
            pointList.Add(currentPosition);
            newLineRenderer.positionCount++;
            newLineRenderer.SetPositions(pointList.ToArray());
            freeDrawPointEmptyTransforms.Add(newFreeDrawPointEmpty.transform);
            previousPosition = currentPosition;

            // Adds a brushPoint at start of line (FreeDrawLineGenerator is point at end of line)
            if (newFreeDrawPointEmpty.transform.position != newLineRenderer.GetPosition(0))
            {
                newFreeDrawPointEmpty.SetActive(false);
                //newFreeDrawPointEmpty.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        
    }
}