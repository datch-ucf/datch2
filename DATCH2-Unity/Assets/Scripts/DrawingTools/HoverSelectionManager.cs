using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using System.Collections;
using UnityEngine;

/// <summary>
/// Allows the user to select shapes based on focus (hand point or camera point)
/// LOCATION: Shape prefab. (Note: As of now, focusHandler only works when attached to the object being focused on.)
/// </summary>
/// 

public class HoverSelectionManager : MonoBehaviour
{
    public BoxDisplayConfiguration customBoxDisplayConfig;
    public Material customBoundingBoxSelectedMaterial;
    public bool isSelected = false;
    private float distanceFromHand;
    private GameObject[] shapesArray;
    private GameObject gestureContainer;
    private float hoverDistance = 0.1f;
    public float hoverSelectDelay = 1.5f;
    public GameObject selectionTimer;
    public bool isHovering = false;
    public bool timerStarted = false;
    public GameObject[] cursorVisuals;
    public bool firstShowCursor;

    void Start()
    {
        gestureContainer = Camera.main.transform.GetChild(0).gameObject;
        shapesArray = gestureContainer.GetComponent<SelectionManager>().shapesArray;
        SelectThisShapeOnCreate(); // Whenever a shape is created, it becomes the new selectedShape in SelectionManager. Other shapes can be selected OnHover as well.
        selectionTimer = GameObject.Find("SelectionTimer");
    }

    //// Call OnTouchEnter
    //public void SelectShapeOnTouch()
    //{
    //    if (selectionTimer == null)
    //    {
    //        selectionTimer = GameObject.Find("SelectionTimer");
    //    }

    //    //isHovering = true;
    //    StartCoroutine(WaitToSelectShapeOnTouch());
    //}

    // Call OnHoverEnter
    public void SelectShapeOnHover(ManipulationEventData eventData)
    {
        if (selectionTimer == null)
        {
            selectionTimer = GameObject.Find("SelectionTimer");

        }

        isHovering = true;
        StartCoroutine(WaitToSelectShape(eventData));
    }

    // Call OnHoverExit
    public void StopSelectionIndicatorProgress()
    {
        isHovering = false;
        selectionTimer.GetComponent<HoverSelectionIndicator>().fillingSelectionTimer = false;
    }

    void Update()
    {
        if (isHovering == true)
        {
            // update stored cursorVisuals array if hovering over shape (whether selected or unselected)
            cursorVisuals = GameObject.FindGameObjectsWithTag("CursorVisual");

            // Cursor visuals do not show properly on first hover. This flag prevents the issue.
            if (firstShowCursor == false)
            {
                ShowCursor();
                firstShowCursor = true;
            }

            ShowCursor();
        }

        if (CoreServices.InputSystem.FocusProvider.PrimaryPointer != null)
        {
            // Distance from hand is based on the difference between the z position of the shape and the user's hand. 
            distanceFromHand = Mathf.Abs(CoreServices.FocusProvider.PrimaryPointer.Position.z - gameObject.transform.position.z); // Most effective?
            //distanceFromHand = Mathf.Abs(Vector3.Distance(CoreServices.FocusProvider.PrimaryPointer.Position, gameObject.GetComponent<Collider>().bounds.center));
            //distanceFromHand = Mathf.Abs(CoreServices.FocusProvider.PrimaryPointer.Position.z - gameObject.GetComponent<Collider>().bounds.center.z);  // Good approach, but needs some adjustments
        }

        // If hand is close to shape, deactivate its box collider and bounds
        if (distanceFromHand >= hoverDistance && gameObject.GetComponent<BoxCollider>().enabled == false)
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
            gameObject.GetComponent<BoundsControl>().enabled = true;


        }

        // If hand is far from shape, activate its box collider and bounds
        else if (distanceFromHand < hoverDistance && gameObject.GetComponent<BoxCollider>().enabled == true)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.GetComponent<BoundsControl>().enabled = false;
        }
    }

    IEnumerator WaitToSelectShape(ManipulationEventData eventData)
    {
        // Only show selectionTimer if shape being focused on is not current selectedShape
        if (isSelected == false && selectionTimer.GetComponent<HoverSelectionIndicator>().fillingSelectionTimer == false && (gestureContainer.GetComponent<SelectionManager>().pointerUp == true || gestureContainer.GetComponent<GestureDetector>().isPinching == false))
        {
            selectionTimer.GetComponent<HoverSelectionIndicator>().fillingSelectionTimer = true;

            yield return new WaitForSeconds(hoverSelectDelay);  // Wait a few seconds to see if user is maintaining focus on shape

            // User must not be pinching and should be hovering over object with open hand to select
            if (eventData.Pointer != null && (gestureContainer.GetComponent<SelectionManager>().pointerUp == true || gestureContainer.GetComponent<GestureDetector>().isPinching == false))
            {
                // If pointer target is a shape && is different from the current selected shape && selectionIndicator is full, select this shape
                if (eventData.Pointer.Result.CurrentPointerTarget == gameObject && selectionTimer.GetComponent<HoverSelectionIndicator>().selectionTimer.fillAmount == 1)
                {
                    DeactivateBoundsAllShapes();
                    isSelected = true;
                    //gestureContainer.GetComponent<SelectionManager>().selectedShape = gameObject;
                    ActivateThisShapeBounds();
                }

                if (isSelected == true)
                {
                    gestureContainer.GetComponent<SelectionManager>().selectedShape = gameObject;

                    StoreSelectedShapeType();
                }
            }
        }
    }

    //IEnumerator WaitToSelectShapeOnTouch()
    //{
    //    // Only show selectionTimer if shape being focused on is not current selectedShape
    //    if (isSelected == false && selectionTimer.GetComponent<HoverSelectionIndicator>().fillingSelectionTimer == false && (gestureContainer.GetComponent<SelectionManager>().pointerUp == true || gestureContainer.GetComponent<GestureDetector>().isPinching == false))
    //    {
    //        print("Iis selected");
    //        selectionTimer.GetComponent<HoverSelectionIndicator>().fillingSelectionTimer = true;

    //        yield return new WaitForSeconds(hoverSelectDelay);  // Wait a few seconds to see if user is maintaining focus on shape

    //        print("POINTER UP: " + gestureContainer.GetComponent<SelectionManager>().pointerUp);
    //        print("IS PINCHING: " + gestureContainer.GetComponent<GestureDetector>().isPinching);
    //        // User must not be pinching and should be hovering over object with open hand to select
    //        //if (gestureContainer.GetComponent<SelectionManager>().pointerUp == true || gestureContainer.GetComponent<GestureDetector>().isPinching == false)
    //        //{
    //            print(selectionTimer.GetComponent<HoverSelectionIndicator>().selectionTimer.fillAmount);
    //            // If pointer target is a shape && is different from the current selected shape && selectionIndicator is full, select this shape
    //            if (selectionTimer.GetComponent<HoverSelectionIndicator>().selectionTimer.fillAmount == 1)
    //            {
    //                DeactivateBoundsAllShapes();
    //                isSelected = true;
    //                //gestureContainer.GetComponent<SelectionManager>().selectedShape = gameObject;
    //                ActivateThisShapeBounds();
    //            }

    //            if (isSelected == true)
    //            {
    //                gestureContainer.GetComponent<SelectionManager>().selectedShape = gameObject;

    //                StoreSelectedShapeType();
    //            }
    //        //}
    //    }
    //}

    public void SelectThisShapeOnCreate()
    {
        DeactivateBoundsAllShapes();
        isSelected = true;
        ActivateThisShapeBounds();

        if (isSelected == true)
        {
            gestureContainer.GetComponent<SelectionManager>().selectedShape = gameObject;

            StoreSelectedShapeType();
        }
    }

    public void DeselectShape(GameObject shape)
    {
        shape.GetComponent<HoverSelectionManager>().isSelected = false;

    }

    public void ActivateThisShapeBounds()
    {
        // Enables bounds -- Selected shape will have a highlighted bounding box
        gameObject.GetComponent<BoundsControl>().BoxDisplayConfig = Instantiate(customBoxDisplayConfig); // can remove if green outline is unneeded
        gameObject.GetComponent<BoundsControl>().Active = true;
        gameObject.GetComponent<BoundsControl>().BoundsControlActivation = Microsoft.MixedReality.Toolkit.UI.BoundsControlTypes.BoundsControlActivationType.ActivateByProximityAndPointer;
        gameObject.GetComponent<BoundsControl>().BoxDisplayConfig.BoxMaterial = customBoundingBoxSelectedMaterial;  // can remove if green outline is unneeded
        gameObject.GetComponent<BoundsControl>().BoxDisplayConfig.BoxGrabbedMaterial = customBoundingBoxSelectedMaterial;  // can remove if green outline is unneeded

        // Allows users to move and rotate shape if it is the selected shape
        gameObject.GetComponent<MoveAxisConstraint>().enabled = false;
        gameObject.GetComponent<RotationAxisConstraint>().enabled = false;

        // Enables manipulation of points inside selected shape
        ObjectManipulator[] objManipulators = gameObject.GetComponentsInChildren<ObjectManipulator>();

        for (int i = 0; i < objManipulators.Length; i++)
        {
            if (i != 0)
            {
                objManipulators[i].enabled = true;
            }
        }
    }

    public void DeactivateShapeBounds(GameObject shape)
    {
        //// Disables bounds -- All unselected shapes will have a default bounding box
        shape.GetComponent<BoundsControl>().BoxDisplayConfig = customBoxDisplayConfig;  // can remove if highlighted box color is unneeded
        shape.GetComponent<BoundsControl>().Active = false;
        shape.GetComponent<BoundsControl>().BoundsControlActivation = Microsoft.MixedReality.Toolkit.UI.BoundsControlTypes.BoundsControlActivationType.ActivateManually;

        // Prevents users from accidentally moving or rotating an unselected shape
        shape.GetComponent<MoveAxisConstraint>().enabled = true;
        shape.GetComponent<RotationAxisConstraint>().enabled = true;

        // Prevents users from moving points of unselected shapes
        ObjectManipulator[] objManipulators = shape.GetComponentsInChildren<ObjectManipulator>();

        for (int i = 0; i < objManipulators.Length; i++)
        {
            if (i != 0)
            {
                objManipulators[i].enabled = false;
            }
        }

    }

    public void DeactivateBoundsAllShapes()
    {
        shapesArray = gestureContainer.GetComponent<SelectionManager>().shapesArray;

        foreach (GameObject shape in shapesArray)
        {
            DeselectShape(shape);
            DeactivateShapeBounds(shape);

        }
    }

    // Call OnHoverEnter()
    public void ShowCursor()
    {
        foreach (GameObject visual in cursorVisuals)
        {
            visual.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    // Call OnHoverExit
    public void HideCursor()
    {
        foreach (GameObject visual in cursorVisuals)
        {
            visual.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void StoreSelectedShapeType()
    {
        if (gameObject.name != "MeasuringCube(Clone)")
        {
            if(gameObject.transform.GetComponentInChildren<DrawFreeDrawLine>() != null)
            {
                gestureContainer.GetComponent<SelectionManager>().selectedShapeType = "FreeDraw";

            }

            if (gameObject.transform.GetComponentInChildren<DrawPolyLine>() != null)
            {
                gestureContainer.GetComponent<SelectionManager>().selectedShapeType = "PolyLine";

            }

            if (gameObject.transform.GetComponentInChildren<DrawBezierCurve4P>() != null)
            {
                gestureContainer.GetComponent<SelectionManager>().selectedShapeType = "Curve";

            }
        }
    }
}
