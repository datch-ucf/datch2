using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;

/// <summary>
/// Creates different objects/components for drawing (freeDrawPoints, drawing planes, etc.). Also controls midair drawing functions.
/// LOCATION: GestureContainer
/// </summary>

public class DrawObjGenerator : MonoBehaviour
{
    public GameObject newDrawComponent = null;
    public GameObject menu;
    public GameObject gestureContainer;
    public GameObject shapePrefab;
    public GameObject shape;
    private GameObject selectedComponent;
    public GameObject polyLineGenerator;
    public GameObject freeDrawGenerator;
    public GameObject curvePrefab;
    public GameObject grabPointPrefab;
    public GameObject generatedGrabPoint;
    public float distanceFromMenu;
    public bool selectedComponentTrackedToHand = false; // Accounts for when a point is actively being tracked to hand as opposed to being grabbed by user. Alternatively, the PointHandler could toggle this bool OnManipulationStart and OnManipulationEnd. Look into later.

    // Pen Variables
    private GameObject pen;
    private GameObject tip;
    private GameObject physicalPenTip;

    // Preventing creation of unwanted points
    public GameObject drawingPlane;
    public float distanceFromDrawingPlane;
    public bool pinchingWDominantHand = false;
    //public GameObject imagePlane;
    //public float distanceFromImagePlane;
    //public List<GameObject> imagePlanesList;



    void Start()
    {
        pen = Camera.main.transform.GetChild(1).gameObject;
        tip = pen.GetComponent<PenTool>().tip;
        physicalPenTip = tip.transform.parent.gameObject;

        SetPrefabSolvers();
    }

    // Setting all pre1abs to follow the correct transform when drawings are created
    public void SetPrefabSolvers()
    {
        freeDrawGenerator.GetComponent<SolverHandler>().TrackedTargetType = TrackedObjectType.CustomOverride;
        freeDrawGenerator.GetComponent<SolverHandler>().TransformOverride = pen.GetComponent<PenTool>().tip.transform;

        grabPointPrefab.GetComponent<SolverHandler>().TrackedTargetType = TrackedObjectType.CustomOverride;
        grabPointPrefab.GetComponent<SolverHandler>().TransformOverride = pen.GetComponent<PenTool>().tip.transform;

        curvePrefab.GetComponentInChildren<SolverHandler>().TrackedTargetType = TrackedObjectType.CustomOverride;
        curvePrefab.GetComponentInChildren<SolverHandler>().TransformOverride = pen.GetComponent<PenTool>().tip.transform;
    }

    void Update()
    {
        
        if (drawingPlane == null)
        {
            drawingPlane = GameObject.Find("DrawingPlane(Clone)");
        }

        //if (drawingPlane != null)
        //{
        //    distanceFromDrawingPlane = Vector3.Distance(drawingPlane.transform.position, pen.transform.position);
        //}

        //if(imagePlane != null)
        //{
        //    distanceFromImagePlane = Vector3.Distance(imagePlane.transform.position, pen.transform.position);
        //}

        //if(imagePlane != null)
        //{
        //    distanceFromImagePlane = Vector3.Distance(imagePlane.transform.position, pen.transform.position);
        //}

        //if(imagePlane != null)
        //{
        //    distanceFromImagePlane = Vector3.Distance(imagePlane.transform.position, pen.transform.position);
        //}

        if (CoreServices.FocusProvider.PrimaryPointer != null)
        {
            distanceFromMenu = Vector3.Distance(menu.transform.position, CoreServices.FocusProvider.PrimaryPointer.Position);
        }

        if (gameObject.GetComponent<GestureDetector>().isPinching == false)
        {
            DeactivateMidAirPoint(); // Stops current point from following pen
            // hides the pen visual, but keeps it active
            pen.GetComponentInChildren<MeshRenderer>().enabled = false;
            pen.GetComponent<PenTool>().physicalPenTip.GetComponentInChildren<MeshRenderer>().enabled = false;
        }

        // if pinching in air or pointing at spatial mesh or pinching drawing plane, show pen
        if (CoreServices.FocusProvider.PrimaryPointer != null)
        {
            if ((gameObject.GetComponent<GestureDetector>().isPinching == true) && pen.GetComponentInChildren<MeshRenderer>().enabled == false && (CoreServices.FocusProvider.PrimaryPointer.Result.CurrentPointerTarget == null || CoreServices.FocusProvider.PrimaryPointer.Result.CurrentPointerTarget.layer == 31 || (selectedComponent != null && selectedComponent.CompareTag("DrawingPlane"))))
            {
                // shows the pen visual when user is pinching
                pen.GetComponentInChildren<MeshRenderer>().enabled = true;
                pen.GetComponent<PenTool>().physicalPenTip.GetComponentInChildren<MeshRenderer>().enabled = true;
            }
        }
        // Hides pen if user is manipulating a shape handle
        if (gestureContainer.GetComponent<SelectionManager>().selectedComponent != null)
        {

            if (gestureContainer.GetComponent<SelectionManager>().selectedComponent.CompareTag("Untagged")) // if user selects a handle
            {
                pen.GetComponentInChildren<MeshRenderer>().enabled = false;
                pen.GetComponent<PenTool>().physicalPenTip.GetComponentInChildren<MeshRenderer>().enabled = false;

            }

        }

        // Hides pen if dominant hand is not present in scene
        if(CoreServices.FocusProvider.PrimaryPointer != null)
        {
            

            if ((CoreServices.FocusProvider.PrimaryPointer.InputSourceParent.SourceName == "Simulated Articulated Left Hand" && menu.GetComponent<MenuOrientation>().dominantHand == Handedness.Right)
            || (CoreServices.FocusProvider.PrimaryPointer.InputSourceParent.SourceName == "Simulated Articulated Right Hand" && menu.GetComponent<MenuOrientation>().dominantHand == Handedness.Left))
            {
                pen.GetComponentInChildren<MeshRenderer>().enabled = false;
                pen.GetComponent<PenTool>().physicalPenTip.GetComponentInChildren<MeshRenderer>().enabled = false;
            }

            // Hides pen if no hands are present in scene
            if (CoreServices.FocusProvider.PrimaryPointer.InputSourceParent.SourceName == "Gaze" || CoreServices.FocusProvider.PrimaryPointer.InputSourceParent.SourceName == "Simulated GGV None Hand")
            {
                pen.GetComponentInChildren<MeshRenderer>().enabled = false;
                pen.GetComponent<PenTool>().physicalPenTip.GetComponentInChildren<MeshRenderer>().enabled = false;
            }
        }
    }

    // For creating midair points OnPointerDown
    public void InstantiateAndPositionOnPinch(MixedRealityPointerEventData eventData)
    {
        selectedComponent = gestureContainer.GetComponent<SelectionManager>().selectedComponent;

        // CREATING POINTS IN MIDAIR

        //Prevents creation of points when pinching with non-dominant hand 
        if(menu.GetComponent<MenuOrientation>().dominantHand == Handedness.Left && (eventData.InputSource.SourceName == "Simulated Articulated Left Hand" || eventData.InputSource.SourceName == "ArticulatedHand Controller Left"))
        {
            pinchingWDominantHand = true;
        }
        if (menu.GetComponent<MenuOrientation>().dominantHand == Handedness.Right && (eventData.InputSource.SourceName == "Simulated Articulated Right Hand" || eventData.InputSource.SourceName == "ArticulatedHand Controller Right"))
        {
            pinchingWDominantHand = true;
        }

        if (menu.GetComponent<MenuOrientation>().dominantHand == Handedness.Left && (eventData.InputSource.SourceName == "Simulated Articulated Right Hand" || eventData.InputSource.SourceName == "ArticulatedHand Controller Right")
            || menu.GetComponent<MenuOrientation>().dominantHand == Handedness.Right && (eventData.InputSource.SourceName == "Simulated Articulated Left Hand" || eventData.InputSource.SourceName == "ArticulatedHand Controller Left"))
        {
            pinchingWDominantHand = false;
        }

        //if(pinchingWDominantHand == true)
        //{
        //    print("PWDH");
        //}

        //if(eventData.Pointer.Result.CurrentPointerTarget == null || eventData.Pointer.Result.CurrentPointerTarget.layer == 31)
        //{
        //    print("FOCUS ON NOTHING");
        //}

        // If pointer target not a selectable object (air) or is spatial mesh, create a new point attached to the pen tip.
        // distanceFromMenu checks whether user is attempting to draw close to menu. Point will NOT be drawn if user is too close (prevents accidental points from being drawn while grabbing sliders and miniPegGrid).
        if (pinchingWDominantHand == true && (eventData.Pointer.Result.CurrentPointerTarget == null || eventData.Pointer.Result.CurrentPointerTarget.layer == 31) && distanceFromMenu > 0.15f && gameObject.GetComponent<GestureDetector>().isPinching == true) // Note: isPinching bool was replaced by isGrabbing == false... Pinch detection seems more responsive this way
        {
            var penTipPos = tip.transform.position;
            var pointerRot = eventData.Pointer.Rotation;

            // Create a midair freeDrawGenerator
            if (gestureContainer.GetComponent<ModeManager>().freeDraw == true)
            {
                // Only FreeDrawPoints need a delay before being generated
                StartCoroutine(DelayStartFreeDrawMidAir(eventData, penTipPos, pointerRot));
            }

            // Create FIRST midair grabPoint for line
            else if (gestureContainer.GetComponent<ModeManager>().line == true && (newDrawComponent == null || newDrawComponent.transform.parent.GetComponentInChildren<LineRenderer>().loop == true))  // else if polyLineGenerator has not yet been created or previous polyLine shape is closed
            {
                StartPolyLineMidAir(eventData, penTipPos, pointerRot);
            }

            // Create subsequent midair grabPoint for line
            else if (gestureContainer.GetComponent<ModeManager>().line == true && newDrawComponent != null)  // polyLineGenerator (first point) has been created
            {
                ContinuePolyLine(eventData, penTipPos, pointerRot);
            }

            // Create midair curve
            else if (gestureContainer.GetComponent<ModeManager>().curve == true)
            {
                StartCurveMidAir(eventData, penTipPos, pointerRot);
            }
        }

        // CREATING POINTS ON DRAWING PLANES
        //else if (selectedComponent.CompareTag("DrawingPlane") && selectedComponent.GetComponent<DrawingPlaneManager>().drawOnPlane == true) // if user clicks a drawing plane and DrawOnPlane checkbox is checked
        //Additional pointer target tags keep user from creating accidental points when grabbing points on drawing plane
        else if (physicalPenTip.transform.GetChild(0).GetComponent<PenTipHelper>().hittingDrawingPlane == true && drawingPlane.transform.GetChild(0).GetComponent<DrawingPlaneManager>().drawOnPlane == true && (eventData.Pointer.Result.CurrentPointerTarget.CompareTag("GrabPoint") == false && eventData.Pointer.Result.CurrentPointerTarget.CompareTag("BezierControlPoint") == false)) // if user clicks a drawing plane and DrawOnPlane checkbox is checked
        {
            var planeCollisionPos = physicalPenTip.transform.GetChild(0).GetComponent<PenTipHelper>().drawingPlaneCollisionPoint;

            // Create a plane-constrained freeDrawGenerator
            if (gestureContainer.GetComponent<ModeManager>().freeDraw == true)
            {
                //StartFreeDrawOnPlane(eventData, planeCollisionPos, cursorRot);
                StartCoroutine(DelayStartFreeDrawOnPlane(eventData, planeCollisionPos));
            }

            // Create FIRST plane-constrained grabpoint for a polyLine
            else if (gestureContainer.GetComponent<ModeManager>().line == true && (newDrawComponent == null || newDrawComponent.transform.parent.GetComponentInChildren<LineRenderer>().loop == true))  // polyLineGenerator has not yet been created
            {
                StartPolyLineOnPlane(eventData, planeCollisionPos);

            }

            // Generate a subsequent plane-constrained grabPoint for line
            else if (gestureContainer.GetComponent<ModeManager>().line == true && newDrawComponent != null)  // polyLineGenerator (first point) has been created
            {
                ContinuePolyLineOnPlane(eventData, planeCollisionPos);
            }

            // Generate a plane-constrained curve
            else if (gestureContainer.GetComponent<ModeManager>().curve == true)
            {
                StartCurveOnPlane(eventData, planeCollisionPos);
            }
        }

    }

    // InstantiateAndPositionOnPinch() is only called OnPointerDown. This coroutine delays the creation of the points to give the Gesture Detector enough time to determine hand position. (Prevents accidental creation of points).
    IEnumerator DelayInstantiateAndPositionOnPinch(MixedRealityPointerEventData eventData)
    {
        // 0.025s may be the smallest yield time that works successfully
        yield return new WaitForSeconds(0.025f); 
    
        InstantiateAndPositionOnPinch(eventData);
    }

    // Calls coroutine
    public void CallDelayInstantantiateAndPositionOnPinch(MixedRealityPointerEventData eventData)
    {
        StartCoroutine(DelayInstantiateAndPositionOnPinch(eventData));
    }

    public void StartFreeDrawMidAir(MixedRealityPointerEventData eventData, Vector3 penTipPos, Quaternion pointerRot)
    {
        newDrawComponent = Instantiate(freeDrawGenerator, penTipPos, pointerRot);
        gestureContainer.GetComponent<SelectionManager>().selectedComponent = newDrawComponent;
        StartCoroutine(WaitToUpdateShapesList());

        // Make freeDrawPoint follow pen tip
        newDrawComponent.GetComponent<SolverHandler>().TransformOverride = tip.transform;
        newDrawComponent.GetComponent<SolverHandler>().enabled = true;
        newDrawComponent.GetComponent<RadialView>().MoveLerpTime = 0.0f; // Initial lerp is at 0 to make freeDrawPoint appear at fingertip
        newDrawComponent.GetComponent<RadialView>().RotateLerpTime = 0.0f;
        newDrawComponent.GetComponent<RadialView>().enabled = true;
        selectedComponentTrackedToHand = true;
        StartCoroutine(WaitToChangeLerpTime(newDrawComponent));  // Sets lerp to 0.075 after a delay to create smooth lines
        
    }

    // FreeDrawPoints must be delayed because the penTip takes additional time to fall into position. This Coroutine helps add a slight delay, preventing unwanted line start
    IEnumerator DelayStartFreeDrawMidAir(MixedRealityPointerEventData eventData, Vector3 penTipPos, Quaternion pointerRot)
    {
        yield return new WaitForSeconds(0.15f);
        penTipPos = tip.transform.position;
        StartFreeDrawMidAir(eventData, penTipPos, pointerRot);
    }
    // First polyLine grabPoint will follow pen tip
    public void StartPolyLineMidAir(MixedRealityPointerEventData eventData, Vector3 penTipPos, Quaternion pointerRot)
    {
        if (generatedGrabPoint != null)
        {
            generatedGrabPoint = null;
        }
        newDrawComponent = Instantiate(polyLineGenerator, penTipPos, pointerRot);
        newDrawComponent.transform.position = penTipPos;
        shape = Instantiate(shapePrefab, penTipPos, pointerRot); // change shape position
        newDrawComponent.transform.parent = shape.transform;
        selectedComponentTrackedToHand = true;

        StartCoroutine(WaitUntilChildIsGenerated(newDrawComponent, eventData)); // Calling this coroutine for first point b/c it is instantiated on Start() of DrawPolyLine(). Returns null otherwise. This handles handedness as well.
        StartCoroutine(WaitToUpdateShapesList());
    }

    // Subsequent polyLine grabPoints will follow pen tip
    public void ContinuePolyLine(MixedRealityPointerEventData eventData, Vector3 penTipPos, Quaternion pointerRot)
    {
        if (generatedGrabPoint != null)
        {
            generatedGrabPoint = null;
        }
        newDrawComponent.GetComponent<DrawPolyLine>().AddPointToLine();
        generatedGrabPoint = newDrawComponent.GetComponent<DrawPolyLine>().newGrabPoint;
        gestureContainer.GetComponent<SelectionManager>().selectedComponent = generatedGrabPoint.gameObject;

        // Make point follow pen tip
        generatedGrabPoint.GetComponent<SolverHandler>().TransformOverride = tip.transform;
        generatedGrabPoint.GetComponent<SolverHandler>().enabled = true;
        generatedGrabPoint.GetComponent<RadialView>().MoveLerpTime = 0.0f; // Initial lerp is at 0 to make freeDrawPoint appear at fingertip
        generatedGrabPoint.GetComponent<RadialView>().RotateLerpTime = 0.0f;
        generatedGrabPoint.GetComponent<RadialView>().enabled = true;
        selectedComponentTrackedToHand = true;

        StartCoroutine(WaitToChangeLerpTime(generatedGrabPoint));  // Sets lerp to 0.075 after a delay to create smooth lines
    }

    public void StartCurveMidAir(MixedRealityPointerEventData eventData, Vector3 penTipPos, Quaternion pointerRot)
    {
        shape = Instantiate(shapePrefab, penTipPos, pointerRot);
        newDrawComponent = Instantiate(curvePrefab, penTipPos, pointerRot);
        newDrawComponent.transform.parent = shape.transform;
        gestureContainer.GetComponent<SelectionManager>().selectedComponent = newDrawComponent.transform.GetChild(0).gameObject; // first child is the first grabPoint on the curve
        StartCoroutine(WaitToUpdateShapesList());

        // Make point follow pen tip
        newDrawComponent.transform.GetChild(0).GetComponent<SolverHandler>().TransformOverride = tip.transform;
        newDrawComponent.transform.GetChild(0).GetComponent<SolverHandler>().enabled = true;
        newDrawComponent.transform.GetChild(0).GetComponent<RadialView>().MoveLerpTime = 0.0f; // Initial lerp is at 0 to make freeDrawPoint appear at fingertip
        newDrawComponent.transform.GetChild(0).GetComponent<RadialView>().RotateLerpTime = 0.0f;
        newDrawComponent.transform.GetChild(0).GetComponent<RadialView>().enabled = true;
        selectedComponentTrackedToHand = true;

        StartCoroutine(WaitToChangeLerpTime(newDrawComponent.transform.GetChild(0).gameObject));  // Sets lerp to 0.075 after a delay to create smooth lines
    }

    // For now, points generated on planes MUST be placed, then grabbed again to prevent issues with constraints. Fix later to allow points to follow fingertip as with midair drawing.
    public void StartFreeDrawOnPlane(MixedRealityPointerEventData eventData, Vector3 planeCollisionPos, Quaternion cursorRot)
    {
        newDrawComponent = Instantiate(freeDrawGenerator, physicalPenTip.transform.GetChild(0).GetComponent<PenTipHelper>().drawingPlaneCollisionPoint, cursorRot);
        gestureContainer.GetComponent<SelectionManager>().selectedComponent = newDrawComponent;
        StartCoroutine(WaitToUpdateShapesList());

        // Tracks point to pen tip collision point
        newDrawComponent.GetComponent<SolverHandler>().TransformOverride = physicalPenTip.transform.GetChild(0).GetComponent<PenTipHelper>().drawingPlaneCollisionTransform.transform;
        newDrawComponent.GetComponent<SolverHandler>().enabled = true;
        newDrawComponent.GetComponent<RadialView>().MoveLerpTime = 0.0f; // Initial lerp is at 0 to make freeDrawPoint appear at fingertip
        newDrawComponent.GetComponent<RadialView>().RotateLerpTime = 0.0f;
        newDrawComponent.GetComponent<RadialView>().enabled = true;
        selectedComponentTrackedToHand = true;

        StartCoroutine(WaitToChangeLerpTime(newDrawComponent));  // Sets lerp to 0.075 after a delay to create smooth lines

    }

    IEnumerator DelayStartFreeDrawOnPlane(MixedRealityPointerEventData eventData, Vector3 collisionPos)
    {
        yield return new WaitForSeconds(0.15f);
        collisionPos = physicalPenTip.transform.GetChild(0).gameObject.GetComponent<PenTipHelper>().drawingPlaneCollisionPoint;
        StartFreeDrawOnPlane(eventData, collisionPos, Quaternion.identity);
    }

    //IEnumerator DelayStartFreeDrawOnPlane(MixedRealityPointerEventData eventData, Vector3 cursorPos, Quaternion cursorRot)
    //{
    //    print("Delaying");
    //    yield return new WaitForSeconds(0.15f);
    //    cursorPos = tip.transform.position;
    //    Vector3 collisionPos = physicalPenTip.transform.GetChild(0).gameObject.GetComponent<PenTipHelper>().drawingPlaneCollisionPoint;
    //    StartFreeDrawOnPlane(eventData, collisionPos, cursorRot);
    //}

    public void StartPolyLineOnPlane(MixedRealityPointerEventData eventData, Vector3 cursorPos)
    {
        Vector3 collisionPos = physicalPenTip.transform.GetChild(0).GetComponent<PenTipHelper>().drawingPlaneCollisionPoint;

        if (generatedGrabPoint != null)
        {
            generatedGrabPoint = null;
        }
        newDrawComponent = Instantiate(polyLineGenerator, physicalPenTip.transform.GetChild(0).gameObject.GetComponent<PenTipHelper>().drawingPlaneCollisionPoint, Quaternion.identity);
        //newDrawComponent.transform.position = collisionPos;
        shape = Instantiate(shapePrefab, physicalPenTip.transform.GetChild(0).gameObject.GetComponent<PenTipHelper>().drawingPlaneCollisionPoint, Quaternion.identity); // change shape position
        newDrawComponent.transform.parent = shape.transform;

        StartCoroutine(WaitUntilChildIsGeneratedPlane(newDrawComponent, eventData)); // Calling this coroutine for first point b/c it is instantiated on Start() of DrawPolyLine(). Returns null otherwise. This handles handedness as well.
        StartCoroutine(WaitToUpdateShapesList());
    }

    public void ContinuePolyLineOnPlane(MixedRealityPointerEventData eventData, Vector3 cursorPos)
    {

        if (generatedGrabPoint != null)
        {
            generatedGrabPoint = null;
        }

        newDrawComponent.GetComponent<DrawPolyLine>().AddPointToLine();
        generatedGrabPoint = newDrawComponent.GetComponent<DrawPolyLine>().newGrabPoint;
        gestureContainer.GetComponent<SelectionManager>().selectedComponent = generatedGrabPoint.gameObject;

        // Tracks point to pen tip collision point
        generatedGrabPoint.GetComponent<SolverHandler>().TransformOverride = physicalPenTip.transform.GetChild(0).GetComponent<PenTipHelper>().drawingPlaneCollisionTransform.transform;
        generatedGrabPoint.GetComponent<SolverHandler>().enabled = true;
        generatedGrabPoint.GetComponent<RadialView>().MoveLerpTime = 0.0f; // Initial lerp is at 0 to make freeDrawPoint appear at fingertip
        generatedGrabPoint.GetComponent<RadialView>().RotateLerpTime = 0.0f;
        generatedGrabPoint.GetComponent<RadialView>().enabled = true;
        selectedComponentTrackedToHand = true;

        StartCoroutine(WaitToChangeLerpTime(generatedGrabPoint));  // Sets lerp to 0.075 after a delay to create smooth lines

    }

    public void StartCurveOnPlane(MixedRealityPointerEventData eventData, Vector3 cursorPos)
    {
        shape = Instantiate(shapePrefab, cursorPos, Quaternion.identity);
        newDrawComponent = Instantiate(curvePrefab, cursorPos, Quaternion.identity);
        newDrawComponent.transform.parent = shape.transform;
        gestureContainer.GetComponent<SelectionManager>().selectedComponent = newDrawComponent.transform.GetChild(0).gameObject; // first child is the first grabPoint on the curve
        StartCoroutine(WaitToUpdateShapesList());

        // Track point to pen tip collision point
        newDrawComponent.transform.GetChild(0).GetComponent<SolverHandler>().TransformOverride = physicalPenTip.transform.GetChild(0).GetComponent<PenTipHelper>().drawingPlaneCollisionTransform.transform;
        newDrawComponent.transform.GetChild(0).GetComponent<SolverHandler>().enabled = true;
        newDrawComponent.transform.GetChild(0).GetComponent<RadialView>().MoveLerpTime = 0.0f; // Initial lerp is at 0 to make freeDrawPoint appear at fingertip
        newDrawComponent.transform.GetChild(0).GetComponent<RadialView>().RotateLerpTime = 0.0f;
        newDrawComponent.transform.GetChild(0).GetComponent<RadialView>().enabled = true;
        selectedComponentTrackedToHand = true;

        StartCoroutine(WaitToChangeLerpTime(newDrawComponent.transform.GetChild(0).gameObject));  // Sets lerp to 0.075 after a delay to create smooth lines
    }


    // Stops point from following finger of active hand OnPointerUp
    public void DeactivateMidAirPoint()
    {

        if (newDrawComponent != null)
        {
            if (newDrawComponent.CompareTag("FreeDrawPoint"))
            {
                //newDrawComponent.GetComponent<RadialView>().MoveLerpTime = 1.0f;  // Attempting to reduce slight line distortion due to lag
                newDrawComponent.GetComponent<SolverHandler>().enabled = false;
                newDrawComponent.GetComponent<RadialView>().enabled = false;
                selectedComponentTrackedToHand = false;
                newDrawComponent = null;
            }

            else if (newDrawComponent.CompareTag("GrabPoint"))
            {

                if (generatedGrabPoint != null)
                {
                    //generatedGrabPoint.GetComponent<RadialView>().MoveLerpTime = 1.0f;  // Attempting to reduce slight line distortion due to lag
                    generatedGrabPoint.GetComponent<SolverHandler>().enabled = false;
                    generatedGrabPoint.GetComponent<RadialView>().enabled = false;
                    selectedComponentTrackedToHand = false;
                }
            }

            else if (newDrawComponent.CompareTag("Curve"))
            {
                if (gestureContainer.GetComponent<SelectionManager>().selectedComponent == newDrawComponent.transform.GetChild(0).gameObject)
                {
                    //newDrawComponent.transform.GetChild(0).GetComponent<RadialView>().MoveLerpTime = 1.0f;  // Attempting to reduce slight line distortion due to lag
                    newDrawComponent.transform.GetChild(0).GetComponent<SolverHandler>().enabled = false;
                    newDrawComponent.transform.GetChild(0).GetComponent<RadialView>().enabled = false;
                    selectedComponentTrackedToHand = false;
                    newDrawComponent = null;

                }
            }
        }
    }

    // When lerp is at 0, point appears at fingertip, but higher lerp is needed afterwards to create smooth lines
    IEnumerator WaitToChangeLerpTime(GameObject point)
    {
        yield return new WaitForSeconds(0.01f);
        //point.GetComponent<RadialView>().MoveLerpTime = 0.075f;
        //point.GetComponent<RadialView>().RotateLerpTime = 0.075f;
        point.GetComponent<RadialView>().MoveLerpTime = 0.085f;
        point.GetComponent<RadialView>().RotateLerpTime = 0.085f;

    }

    // For generating first polyLine grabPoint in air (follows cursor)
    IEnumerator WaitUntilChildIsGenerated(GameObject newDrawComponent, MixedRealityPointerEventData eventData) // Creates a short delay until generatedGrabPoint is created
    {
        yield return new WaitUntil(() => newDrawComponent.GetComponent<DrawPolyLine>().newGrabPoint != null);
        //yield return new WaitForSeconds(1f);
        generatedGrabPoint = newDrawComponent.GetComponent<DrawPolyLine>().newGrabPoint;
        gestureContainer.GetComponent<SelectionManager>().selectedComponent = generatedGrabPoint;
        generatedGrabPoint.GetComponent<SolverHandler>().TransformOverride = tip.transform;
        generatedGrabPoint.GetComponent<SolverHandler>().enabled = true;
        generatedGrabPoint.GetComponent<RadialView>().enabled = true;
        selectedComponentTrackedToHand = true;
    }

    // For generating first polyLine grabPoint on plane (follows cursor)
    IEnumerator WaitUntilChildIsGeneratedPlane(GameObject newDrawComponent, MixedRealityPointerEventData eventData)
    {
        //yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => newDrawComponent.GetComponent<DrawPolyLine>().newGrabPoint != null);
        generatedGrabPoint = newDrawComponent.GetComponent<DrawPolyLine>().newGrabPoint;
        gestureContainer.GetComponent<SelectionManager>().selectedComponent = generatedGrabPoint.gameObject;

        // The following lines allow the point to follow the collision point
        generatedGrabPoint.GetComponent<SolverHandler>().TransformOverride = physicalPenTip.transform.GetChild(0).gameObject.GetComponent<PenTipHelper>().drawingPlaneCollisionTransform.transform;
        generatedGrabPoint.GetComponent<SolverHandler>().enabled = true;
        generatedGrabPoint.GetComponent<RadialView>().MoveLerpTime = 0.0f; // Initial lerp is at 0 to make freeDrawPoint appear at fingertip
        generatedGrabPoint.GetComponent<RadialView>().RotateLerpTime = 0.0f;
        generatedGrabPoint.GetComponent<RadialView>().enabled = true;
        selectedComponentTrackedToHand = true;
    }

    IEnumerator WaitToUpdateShapesList()
    {
        yield return new WaitForSeconds(0.1f);
        gestureContainer.GetComponent<SelectionManager>().UpdateShapesArray();

    }
}