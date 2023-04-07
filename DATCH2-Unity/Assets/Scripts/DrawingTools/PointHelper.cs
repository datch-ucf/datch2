using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script prevents issues with point sizing when shape scale is changed with bounds handles. It also helps created points stick to drawing plane surface.
/// LOCATION: FreeDrawPoints, GrabPoints, EndGrabPoints
/// </summary>
public class PointHelper : MonoBehaviour
{
    private Vector3 scale;
    private GameObject gestureContainer;
    private GameObject pen;
    private GameObject physicalPenTip;
    [SerializeField] private bool lockedToPlane;
    private Vector3 snapPos;
    private bool isGrabbed = false;
    private GameObject collisionPlane;
    public float distanceFromDrawingPlane;
    public GameObject activeDrawingPlane;
    public GameObject nearestLinkedPoint;
    public bool linkedToPoint = false;
    public GameObject shapeGroupPrefab;
    public GameObject pointShapeGroup;
    public bool isGrouped;
    public GameObject pointShapeParent;
    public Transform[] objectsInGroup; 

    private void Start()
    {
        gestureContainer = Camera.main.transform.GetChild(0).gameObject;
        pen = GameObject.Find("Pen");
        physicalPenTip = pen.GetComponent<PenTool>().physicalPenTip;
        scale = gameObject.transform.localScale;
        FindPointShapeParent();
    }

    void Update()
    {
        // Partially working method for making grabbed points stick to planes. Still has some jittering. Look into later.
        //if(activeDrawingPlane != null)
        //{
        //    distanceFromDrawingPlane = Vector3.Distance(gameObject.transform.position, activeDrawingPlane.transform.position);

        //    if (distanceFromDrawingPlane < 0.5f)
        //    {
        //        // Re-orient point's rotation to match the orientation of the drawing plane
        //        gameObject.transform.localRotation = activeDrawingPlane.transform.parent.localRotation;
        //        //gameObject.GetComponent<ObjectManipulator>().SmoothingFar = false;
        //        //gameObject.GetComponent<ObjectManipulator>().SmoothingNear = false;
        //        //gameObject.transform.position = physicalPenTip.transform.GetChild(0).GetComponent<PenTipHelper>().drawingPlaneCollisionPoint;
        //    }
        //}

        gameObject.transform.localScale = AdditionalUtilityFunctions.SetGlobalScale(gameObject, gameObject.transform, scale);

        // Point linking -- keeps the current point attached to the nearest linked point (detach with a swift jerk to break connection)
        if (linkedToPoint == true)
        {
            gameObject.transform.position = nearestLinkedPoint.transform.position;

            //// If a pointShapeGroup doesn't exist for this point, create a new one
            //if (pointShapeGroup == null)
            //{
                //shape is a freeDraw shape
                //if (gameObject.CompareTag("FreeDrawPoint"))
                //{
                //    pointShapeGroup = Instantiate(shapeGroupPrefab);

                //    pointShapeParent.transform.SetParent(pointShapeGroup.transform);

                //    nearestLinkedPoint.GetComponent<PointHelper>().pointShapeParent.transform.SetParent(pointShapeGroup.transform);
                //}

                //else if (gameObject.CompareTag("GrabPoint"))
                //{
                //    // Shape is a polyLine
                //    if (gameObject.transform.parent.CompareTag("Curve") == false)
                //    {
                //        pointShapeGroup = Instantiate(shapeGroupPrefab);

                //        pointShapeParent.transform.SetParent(pointShapeGroup.transform);

                //        nearestLinkedPoint.GetComponent<PointHelper>().pointShapeParent.transform.SetParent(pointShapeGroup.transform);

                //}

                //// Shape is a curve
                //else if (gameObject.transform.parent.CompareTag("Curve"))
                //    {
                //        pointShapeGroup = Instantiate(shapeGroupPrefab);

                //        pointShapeParent.transform.SetParent(pointShapeGroup.transform);

                //        nearestLinkedPoint.GetComponent<PointHelper>().pointShapeParent.transform.SetParent(pointShapeGroup.transform);

                //}
            //}
                //    FindPointShapeParent();

                //    // shape is a freeDraw shape
                //    if (gameObject.CompareTag("FreeDrawPoint"))
                //    {
                //        pointShapeGroup = Instantiate(shapeGroupPrefab, gameObject.transform.parent);

                //        pointShapeParent.transform.SetParent(pointShapeGroup.transform);
                //    }
                //    if (gameObject.CompareTag("GrabPoint"))
                //    {
                //        // Shape is a polyLine
                //        if (gameObject.transform.parent.CompareTag("Curve") == false)
                //        {
                //            pointShapeGroup = Instantiate(shapeGroupPrefab, gameObject.transform.parent);

                //            pointShapeParent.transform.SetParent(pointShapeGroup.transform);

                //        }

                //        // Shape is a curve
                //        if (gameObject.transform.parent.CompareTag("Curve"))
                //        {
                //            pointShapeGroup = Instantiate(shapeGroupPrefab, gameObject.transform.parent);

                //            pointShapeParent.transform.SetParent(pointShapeGroup.transform);
                //        }
                //    }
                //}
            //}
        }
    }

    private void FindPointShapeParent()
    {
        // shape is a freeDraw shape
        if (gameObject.CompareTag("FreeDrawPoint"))
        {
            pointShapeParent = gameObject.transform.parent.gameObject;
        }
        if (gameObject.CompareTag("GrabPoint"))
        {
            // Shape is a polyLine
            if (gameObject.transform.parent.CompareTag("Curve") == false)
            {
                pointShapeParent = gameObject.transform.parent.gameObject;
            }

            // Shape is a curve
            else
            {
                pointShapeParent = gameObject.transform.parent.parent.gameObject;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DrawingPlane"))
        {
            activeDrawingPlane = other.gameObject;
            // Condition occurs if point is brought back into the collision box of drawing plane (re-entry)
            if (lockedToPlane == true)
            {
                lockedToPlane = false;
            }
        }

        // Enables point linking
        if((other.CompareTag("FreeDrawPoint") || other.CompareTag("GrabPoint") || other.CompareTag("EndGrabPoint")) && other.transform.parent != gameObject.transform.parent && gameObject == gestureContainer.GetComponent<SelectionManager>().selectedComponent) // Last condition only enables linking of currently selected point (deliberately placed)
        //if((other.CompareTag("FreeDrawPoint") || other.CompareTag("GrabPoint") || other.CompareTag("EndGrabPoint")) && other.transform.parent != gameObject.transform.parent) // Last condition only enables linking of currently selected point (deliberately placed)
        {
            //print("COLLIDED WITH: " + other.gameObject.tag);
            nearestLinkedPoint = other.gameObject;
            gameObject.transform.position = nearestLinkedPoint.transform.position;
            linkedToPoint = true;
            // Added
            nearestLinkedPoint.GetComponent<PointHelper>().linkedToPoint = true;
            nearestLinkedPoint.GetComponent<PointHelper>().nearestLinkedPoint = gameObject;
            //if (pointShapeGroup != null || nearestLinkedPoint.GetComponent<PointHelper>().pointShapeGroup != null)
            //{
            //    print("EXISTING GROUP");
            //    if (gameObject.CompareTag("FreeDrawPoint"))
            //    {
            //        pointShapeGroup = nearestLinkedPoint.GetComponent<PointHelper>().pointShapeGroup;

            //        pointShapeParent.transform.SetParent(pointShapeGroup.transform);

            //        //nearestLinkedPoint.GetComponent<PointHelper>().pointShapeParent.transform.SetParent(pointShapeGroup.transform);
            //    }

            //    else if (gameObject.CompareTag("GrabPoint"))
            //    {
            //        // Shape is a polyLine
            //        if (gameObject.transform.parent.CompareTag("Curve") == false)
            //        {
            //            //pointShapeGroup = Instantiate(shapeGroupPrefab);
            //            pointShapeGroup = nearestLinkedPoint.GetComponent<PointHelper>().pointShapeGroup;

            //            pointShapeParent.transform.SetParent(pointShapeGroup.transform);

            //            //nearestLinkedPoint.GetComponent<PointHelper>().pointShapeParent.transform.SetParent(pointShapeGroup.transform);
            //            //nearestLinkedPoint.GetComponent<PointHelper>().nearestLinkedPoint.GetComponent<PointHelper>().
            //        }

            //        // Shape is a curve
            //        else if (gameObject.transform.parent.CompareTag("Curve"))
            //        {
            //            //pointShapeGroup = Instantiate(shapeGroupPrefab);

            //            pointShapeGroup = nearestLinkedPoint.GetComponent<PointHelper>().pointShapeGroup;


            //            pointShapeParent.transform.SetParent(pointShapeGroup.transform);

            //            //nearestLinkedPoint.GetComponent<PointHelper>().pointShapeParent.transform.SetParent(pointShapeGroup.transform);

            //        }
            //    }
            //}
            // If a pointShapeGroup doesn't exist for this point or its connected point, create a new one
            //if (pointShapeGroup == null && nearestLinkedPoint.GetComponent<PointHelper>().pointShapeGroup == null)
            //{
            //    //print("NEW GROUP");
            //    if (gameObject.CompareTag("FreeDrawPoint"))
            //    {
            //        pointShapeGroup = Instantiate(shapeGroupPrefab);

            //        pointShapeParent.transform.SetParent(pointShapeGroup.transform);

            //        nearestLinkedPoint.GetComponent<PointHelper>().pointShapeParent.transform.SetParent(pointShapeGroup.transform);
            //    }

            //    else if (gameObject.CompareTag("GrabPoint"))
            //    {
            //        // Shape is a polyLine
            //        if (gameObject.transform.parent.CompareTag("Curve") == false)
            //        {
            //            pointShapeGroup = Instantiate(shapeGroupPrefab);

            //            pointShapeParent.transform.SetParent(pointShapeGroup.transform);

            //            nearestLinkedPoint.GetComponent<PointHelper>().pointShapeParent.transform.SetParent(pointShapeGroup.transform);

            //        }

            //        // Shape is a curve
            //        else if (gameObject.transform.parent.CompareTag("Curve"))
            //        {
            //            pointShapeGroup = Instantiate(shapeGroupPrefab);

            //            pointShapeParent.transform.SetParent(pointShapeGroup.transform);

            //            nearestLinkedPoint.GetComponent<PointHelper>().pointShapeParent.transform.SetParent(pointShapeGroup.transform);

            //        }
            //    }

            //    // If a pointShapeGroup exists
            //    //else if (pointShapeGroup != null)
            //    //{
            //    //    // Unparent current point
            //    //    pointShapeParent.transform.parent = null;
            //    //    // Destroy its previous group
            //    //    Destroy(pointShapeGroup);

            //    //    nearestLinkedPoint.GetComponent<PointHelper>().pointShapeParent = null;
            //    //    //Destroy(nearestLinkedPoint.GetComponent<PointHelper>().pointShapeGroup);

            //    //    //Set its new group to be the same as connected point

            //    //    if (gameObject.CompareTag("FreeDrawPoint"))
            //    //    {
            //    //        pointShapeGroup = Instantiate(shapeGroupPrefab);

            //    //        pointShapeParent.transform.SetParent(pointShapeGroup.transform);

            //    //        nearestLinkedPoint.GetComponent<PointHelper>().pointShapeParent.transform.SetParent(pointShapeGroup.transform);
            //    //    }

            //    //    else if (gameObject.CompareTag("GrabPoint"))
            //    //    {
            //    //        // Shape is a polyLine
            //    //        if (gameObject.transform.parent.CompareTag("Curve") == false)
            //    //        {
            //    //            pointShapeGroup = Instantiate(shapeGroupPrefab);

            //    //            pointShapeParent.transform.SetParent(pointShapeGroup.transform);

            //    //            nearestLinkedPoint.GetComponent<PointHelper>().pointShapeParent.transform.SetParent(pointShapeGroup.transform);

            //    //        }

            //    //        // Shape is a curve
            //    //        else if (gameObject.transform.parent.CompareTag("Curve"))
            //    //        {
            //    //            pointShapeGroup = Instantiate(shapeGroupPrefab);

            //    //            pointShapeParent.transform.SetParent(pointShapeGroup.transform);

            //    //            nearestLinkedPoint.GetComponent<PointHelper>().pointShapeParent.transform.SetParent(pointShapeGroup.transform);
            //    //        }
            //    //    }
            //    //}

            //    //else
            //    //{
            //    //    print("NEED TO FIX CONNECTION");
            //    //}

            //    //else if (pointShapeGroup != null || nearestLinkedPoint.GetComponent<PointHelper>().pointShapeGroup != null)
            //    //{
            //    //    print("NEED TO FIX CONNECTION");
            //    //    print("EXISTING GROUP");
            //    //    if (gameObject.CompareTag("FreeDrawPoint"))
            //    //    {
            //    //        pointShapeGroup = nearestLinkedPoint.GetComponent<PointHelper>().pointShapeGroup;

            //    //        pointShapeParent.transform.SetParent(pointShapeGroup.transform);

            //    //        //nearestLinkedPoint.GetComponent<PointHelper>().pointShapeParent.transform.SetParent(pointShapeGroup.transform);
            //    //    }

            //    //    else if (gameObject.CompareTag("GrabPoint"))
            //    //    {
            //    //        // Shape is a polyLine
            //    //        if (gameObject.transform.parent.CompareTag("Curve") == false)
            //    //        {
            //    //            //pointShapeGroup = Instantiate(shapeGroupPrefab);
            //    //            pointShapeGroup = nearestLinkedPoint.GetComponent<PointHelper>().pointShapeGroup;

            //    //            pointShapeParent.transform.SetParent(pointShapeGroup.transform);

            //    //            //nearestLinkedPoint.GetComponent<PointHelper>().pointShapeParent.transform.SetParent(pointShapeGroup.transform);
            //    //            //nearestLinkedPoint.GetComponent<PointHelper>().nearestLinkedPoint.GetComponent<PointHelper>().
            //    //        }

            //    //        // Shape is a curve
            //    //        else if (gameObject.transform.parent.CompareTag("Curve"))
            //    //        {
            //    //            //pointShapeGroup = Instantiate(shapeGroupPrefab);

            //    //            pointShapeGroup = nearestLinkedPoint.GetComponent<PointHelper>().pointShapeGroup;


            //    //            pointShapeParent.transform.SetParent(pointShapeGroup.transform);

            //    //            //nearestLinkedPoint.GetComponent<PointHelper>().pointShapeParent.transform.SetParent(pointShapeGroup.transform);

            //    //    }
            //    //}
            //    //}

            //    // Removing for now
            //    //ConnectShapesInGroup(pointShapeGroup);
            //    //ConnectShapesInGroup(nearestLinkedPoint.GetComponent<PointHelper>().pointShapeGroup);
            //}
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // For drawing plane snappable gridPoints
        // Only allow snapping if drawOnPlane is enabled
        if (other.gameObject.CompareTag("SnapPoint") && other.gameObject.GetComponentInParent<DrawingPlaneManager>().drawOnPlane == true)
        {
            gameObject.transform.position = other.transform.position;

            // Attempting to display drawing plane collision target
            //physicalPenTip.GetComponent<PenTipHelper>().drawingPlaneCollisionTarget.GetComponentInChildren<Canvas>().enabled = true;
        }
        // if drawOnPlane is not enabled, do not enable snapping (This also prevents points from following the plane when it moves)
        else
        {
            gameObject.transform.position = gameObject.transform.position;
        }

        // Constrain to drawing plane surface when point is tracked to hand
        if (other.CompareTag("DrawingPlane") && gestureContainer.GetComponent<DrawObjGenerator>().selectedComponentTrackedToHand == true)
        {
            collisionPlane = other.gameObject;
            // Re-orient point's rotation to match the orientation of the drawing plane
            gameObject.transform.localRotation = other.transform.parent.localRotation;


            // Give point the same position as the target
            if (gameObject.GetComponent<RadialView>() != null && lockedToPlane == false)
            {
                gameObject.GetComponent<SolverHandler>().TransformOverride = physicalPenTip.transform.GetChild(0).GetComponent<PenTipHelper>().drawingPlaneCollisionTransform.transform;
                gameObject.GetComponent<SolverHandler>().enabled = true;
                gameObject.GetComponent<RadialView>().enabled = true;
                lockedToPlane = true;

                // Keep point constrained to plane's surface  (NOT SURE IF NEEDED b/c the method being used now accounts for SolverHandler and RadialView)
                //gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, other.transform.parent.localPosition.z);
            }

            // If user stops pinching, stop locking to plane
            if (gestureContainer.GetComponent<SelectionManager>().pointerUp == true || gestureContainer.GetComponent<GestureDetector>().isPinching == false)
            {
                DeactivatePlaneLocking();
            }

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DrawingPlane"))
        {
            activeDrawingPlane = null;
            if (gameObject.GetComponent<RadialView>() != null)
            {
                DeactivatePlaneLocking();
            }
        }

        // Attempting to hide display of drawing plane collision target
        //if(other.CompareTag("SnapPoint"))
        //{
        //    physicalPenTip.GetComponent<PenTipHelper>().drawingPlaneCollisionTarget.GetComponentInChildren<Canvas>().enabled = false;
        //}

        // Disables point linking
        if ((other.CompareTag("FreeDrawPoint") || other.CompareTag("GrabPoint") || other.CompareTag("EndGrabPoint")) && other.transform.parent != gameObject.transform.parent)
        {
            nearestLinkedPoint = null;
            linkedToPoint = false;

            // Added
            nearestLinkedPoint.GetComponent<PointHelper>().linkedToPoint = false;
            nearestLinkedPoint.GetComponent<PointHelper>().nearestLinkedPoint = null;
        }
    }

    public void DeactivatePlaneLocking()
    {
        if (gameObject.GetComponent<SolverHandler>() != null && gameObject.GetComponent<RadialView>() != null)
        {
            gameObject.GetComponent<SolverHandler>().enabled = false;
            gameObject.GetComponent<RadialView>().enabled = false;
        }
    }

    public void ConnectShapesInGroup(GameObject inputShapeGroup)
    {
        print("CONEECTING POINTS");
        objectsInGroup = inputShapeGroup.GetComponentsInChildren<Transform>();

        for(int i = 0; i < objectsInGroup.Length; i++)
        {
            if (objectsInGroup[i].gameObject.CompareTag("Shape"))
            {
                objectsInGroup[i].transform.SetParent(inputShapeGroup.transform);
            }
        }
    }


    // ------------------------- Not currently being used ------------------------------------

    // Call OnManipulationStart()
    public void CheckIsGrabbed()
    {
        isGrabbed = true;
    }

    // Call OnManipulationEnd()
    public void UnCheckIsGrabbed()
    {
        isGrabbed = false;
    }
}

