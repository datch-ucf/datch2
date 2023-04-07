using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.UI;


public class DrawPolyLine : MonoBehaviour
{
    public LineRenderer lineRendererOrigin;
    public GameObject grabPoint;
    private List<Vector3> pointList = new List<Vector3>();
    private List<Transform> grabPointTransforms = new List<Transform>();
    private LineRenderer newLineRenderer;
    public GameObject shape;
    private GameObject gestureContainer;
    public GameObject newGrabPoint;
    private Vector3 spawnPosition;
    public GameObject polyLineGenerator;
    public GameObject newPolyLineGenerator;
    private float closeDistance = 0.1f;
    private bool firstPointCreated = false;
    private GameObject pen;
    public bool manualClose = false;

    void Start () 
    {
        gestureContainer = Camera.main.transform.GetChild(0).gameObject;
        pen = Camera.main.transform.GetChild(1).gameObject;
        newLineRenderer = Instantiate(lineRendererOrigin, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        newLineRenderer.useWorldSpace = true; // need to set to true for polyLines for correct behavior on move, rotate, or scale
        shape = gestureContainer.GetComponent<DrawObjGenerator>().shape;
        newLineRenderer.transform.parent = shape.transform;
        AddPointToLine();
    }

    // Update is called once per frame
    void Update ()
	{
        for (int i = 0; i < (pointList.ToArray()).Length; i++)
        {
            newLineRenderer.SetPosition(i, grabPointTransforms[i].position);

            if (Vector3.Distance(grabPointTransforms[i].position, grabPointTransforms[0].position) < closeDistance  && newLineRenderer.positionCount > 2)
            {
                //print("Looping");
                newLineRenderer.loop = enabled;
            }
            else 
            {
                //print("Not looping");
                newLineRenderer.loop = false;
            }
        }

        if (manualClose == true)
        {
            newLineRenderer.loop = true;
        }

        //if(firstPointCreated == true)
        //{
        //    gameObject.transform.position = gameObject

        //}
    }

    public void AddPointToLine()
    {
        if (firstPointCreated == false)
        {
            spawnPosition = gameObject.transform.position;
            firstPointCreated = true;
        }

        else
        {
            if (gestureContainer.GetComponent<SelectionManager>().selectedComponent == null)
            {
                //spawnPosition = CoreServices.FocusProvider.PrimaryPointer.Position;
                spawnPosition = pen.GetComponent<PenTool>().physicalPenTip.transform.GetChild(1).position;

            }
            else if(gestureContainer.GetComponent<SelectionManager>().selectedComponent.tag == "DrawingPlane")
            {
                //spawnPosition = CoreServices.FocusProvider.PrimaryPointer.BaseCursor.Position;
                spawnPosition = pen.GetComponent<PenTool>().physicalPenTip.transform.GetChild(1).position;
            }
        }

        if (manualClose == true)
        {
            newLineRenderer.loop = true;
        }


        var result = CoreServices.FocusProvider.PrimaryPointer.Result;

        newGrabPoint = Instantiate(grabPoint, spawnPosition, Quaternion.LookRotation(result.Details.Normal));
        newGrabPoint.transform.parent = shape.transform;

        pointList.Add(spawnPosition);
        newLineRenderer.positionCount++;
        newLineRenderer.SetPositions(pointList.ToArray());
        grabPointTransforms.Add(newGrabPoint.transform); 
    }
}