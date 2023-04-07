using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using Microsoft.MixedReality.Toolkit.Utilities;
/// <summary>
/// This script is used to adjust the rotation of drawing planes and manage its collisions with drawing points
/// LOCATION: Drawing plane
/// </summary>

public class DrawingPlaneManager : MonoBehaviour
{
    public bool drawOnPlane = false; // this bool and the isToggled variable in Inspector enable plane drawing at start
    public bool gridPointSnapping = false; // will immediately be set to false at start
    //public GameObject label;
    public float xTransOffset = 0.05f;
    public float yTransOffset = -0.13f;
    public float zTransOffset;
    //public GameObject[] drawingPlaneHandles;
    //public List<float> handleDistancesFromPen;
    public GameObject pen;
    public bool changingScale;
    //public GameObject[] cursorVisuals;
    //public bool firstShowCursor;
    //public bool isHovering;
    public GameObject gridLineDistanceCanvas;


    void Start()
    {
        pen = Camera.main.transform.GetChild(1).gameObject;
        //physicalPenTip = pen.GetComponent<PenTool>().physicalPenTip;

        xTransOffset = 0.05f;
        yTransOffset = -0.13f;

        // Initialize this function on start
        ToggleDrawOnPlaneCheckbox();

        // Initializing at start
        gridLineDistanceCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Gridline Distance: " + ((gameObject.GetComponentInChildren<GridObjectCollection>().CellWidth * gameObject.transform.localScale.x) / 10).ToString("F2") +
                                                                                            " x " + ((gameObject.GetComponentInChildren<GridObjectCollection>().CellHeight * gameObject.transform.localScale.y) / 10).ToString("F2") + " m";
    }

    void Update()
    {
        if (changingScale == true)
        {
            gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y);
            gridLineDistanceCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Gridline Distance: " + ((gameObject.GetComponentInChildren<GridObjectCollection>().CellWidth * gameObject.transform.localScale.x) / 10).ToString("F2") +
                                                                                            " x " + ((gameObject.GetComponentInChildren<GridObjectCollection>().CellHeight * gameObject.transform.localScale.y) / 10).ToString("F2") + " m";
        }

        //if (isHovering == true)
        //{
        //    // update stored cursorVisuals array if hovering over shape (whether selected or unselected)
        //    cursorVisuals = GameObject.FindGameObjectsWithTag("CursorVisual");

        //    // Cursor visuals do not show properly on first hover. This flag prevents the issue.
        //    if (firstShowCursor == false)
        //    {
        //        ShowCursor();
        //        firstShowCursor = true;
        //    }

        //    ShowCursor();
        //}
    }

    public void ToggleDrawOnPlaneCheckbox()
    {
        if (drawOnPlane == true)
        {
            print("Toggled Draw on plane false");
            //handleDistancesFromPen.Clear();
            drawOnPlane = false;
            gameObject.GetComponent<BoundsControl>().enabled = true;
            //gameObject.GetComponent<ObjectManipulator>().enabled = true;

            //drawingPlaneHandles = gameObject.transform.GetChild(2).GetComponentsInChildren<GameObject>();
            //foreach (GameObject handle in drawingPlaneHandles)
            //{
            //    float distanceFromPen = Vector3.Distance(handle.transform.position, pen.transform.position);
            //    handleDistancesFromPen.Add(distanceFromPen);
            //}
        }

        else if (drawOnPlane == false)
        {
            print("Toggled Draw on plane true");
            drawOnPlane = true;
            gameObject.GetComponent<BoundsControl>().enabled = false;
            //gameObject.GetComponent<ObjectManipulator>().enabled = false;
        }
    }

    // Call OnScaleStarted
    public void ToggleChangingScaleOn()
    {
        changingScale = true;
    }

    // Call OnScaleStopped
    public void ToggleChangingScaleOff()
    {
        changingScale = false;
    }

    // Disables enclosing box collider to allow for snapping exclusively on gridPoints
    // Enclosing box collider is only active when snapping gridpoint colliders are inactive
    public void ToggleGridPointSnapping()
    {
        // if (gameObject.GetComponent<BoxCollider>().enabled == true)
        if(gridPointSnapping == false)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            foreach (GameObject gridPoint in gameObject.transform.GetComponentInChildren<DrawingPlaneSnappableGrid>().gridPointsArr)
            {
                gridPoint.GetComponent<BoxCollider>().enabled = true;
            }
            gridPointSnapping = true;
        }

        // else if (gameObject.GetComponent<BoxCollider>().enabled == false)
        else if(gridPointSnapping  == true)
        {

            gameObject.GetComponent<BoxCollider>().enabled = true;
            foreach (GameObject gridPoint in gameObject.transform.GetComponentInChildren<DrawingPlaneSnappableGrid>().gridPointsArr)
            {
                gridPoint.GetComponent<BoxCollider>().enabled = false;
            }
            gridPointSnapping = false;
        }
    }

    // Call OnHoverEnter()
    //public void CheckIsHovering()
    //{
    //    isHovering = true;
    //}

    //// Call OnHoverExit
    //public void UnCheckIsHovering()
    //{
    //    isHovering = false;
    //}


    //public void ShowCursor()
    //{
    //    foreach (GameObject visual in cursorVisuals)
    //    {
    //        visual.GetComponent<MeshRenderer>().enabled = true;
    //    }
    //}


    //public void HideCursor()
    //{
    //    foreach (GameObject visual in cursorVisuals)
    //    {
    //        visual.GetComponent<MeshRenderer>().enabled = false;
    //    }
    //}

    // Automatically set drawing plane to 90 degrees from ground
    public void AutoPerpendicular()
    {
        gameObject.transform.localEulerAngles = new Vector3(90, 0, 0);
    }

    // Automatically set drawing plane to 180 degrees from ground
    public void AutoParallel()
    {
        gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
    }
}
