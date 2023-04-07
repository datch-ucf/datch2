using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Microsoft.MixedReality.Toolkit.Input;

/// <summary>
/// Tracks the user's selections. Used for selecting components to delete and for filling drawn shapes.
/// LOCATION: Attach to the gestureContainer gameObject (child of main camera) 
/// </summary>

public class SelectionManager : MonoBehaviour
{
    public GameObject selectedComponent = null;
    public GameObject previousSelectedComponent = null;
    private Interactable activatedButton;
    public GameObject activatedButtonGameObject;
    private List<Interactable> buttonInteractables = new List<Interactable>();
    public GameObject selectedShape;
    public string selectedShapeType;
    public GameObject menu;
    private GameObject[] buttons;
    public GameObject[] shapesArray;
    public bool pointerUp = true;


    void Start()
    {
        //// Disable the hand rays
        //PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);

        //// Disable the gaze pointer
        //PointerUtils.SetGazePointerBehavior(PointerBehavior.AlwaysOff);


        // Collecting information for GetActivatedButton() function
        // Menu and all button collections must be active in order for button objects to be stored
        menu.SetActive(true); // setting Menu active at start to collect list of buttons at start
        menu.transform.GetChild(0).gameObject.SetActive(true);  // setting MenuContentParent active
        menu.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);  // setting MenuPanels active
        menu.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(true); // setting ButtonCollectionFile active
        menu.transform.GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(true); // setting ButtonCollectionTools active
        menu.transform.GetChild(0).GetChild(1).GetChild(2).gameObject.SetActive(true); // setting ButtonCollectionAttributes active
        menu.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);  // setting ScrollPaginationButtons active
        menu.transform.GetChild(0).GetChild(2).GetChild(1).gameObject.SetActive(true);  // setting ButtonLeft active
        buttons = GameObject.FindGameObjectsWithTag("Button"); // Now that all buttons are active, find and store them
        menu.SetActive(true); // setting Menu active at start to collect list of buttons at start
        
        // Disabling button collections to restore menu to correct starting configuration
        menu.transform.GetChild(0).gameObject.SetActive(false);
        menu.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);  // MenuPanels should remain active at start
        menu.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(true); // ButtonCollectionFile should remain active at start
        menu.transform.GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(false);
        menu.transform.GetChild(0).GetChild(1).GetChild(2).gameObject.SetActive(false);
        menu.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);  // ScrollPaginationButtons should remain active at start
        menu.transform.GetChild(0).GetChild(2).GetChild(1).gameObject.SetActive(false);

        // List of button interactables 
        foreach (var button in buttons)
        {
            if (button != null)
            {
                buttonInteractables.Add(button.GetComponent<Interactable>());
            }
        }

        GetActivatedButton();
    }

    // This will be updated in DrawObjGenerator (whenever a shape is created) and in the DeleteShape button (whenever a shape is destroyed)
    public void UpdateShapesArray()
    {
        shapesArray = GameObject.FindGameObjectsWithTag("Shape");
    }

    public void SelectComponent()
    {
        previousSelectedComponent = selectedComponent; // used to store selected component as more components are clicked
        selectedComponent = CoreServices.InputSystem.FocusProvider.PrimaryPointer.Result.CurrentPointerTarget;  // store current pointer target as selected component
        StartCoroutine(ActivateSolverOnSelected()); // waits until selected component is updated and activates its solvers
    }

    public void DeselectShape()
    {
        selectedShape.GetComponent<HoverSelectionManager>().DeactivateBoundsAllShapes();
        selectedShape = null;
        UpdateShapesArray();
    }

    public void GetActivatedButton()
    {
        foreach (var interactable in buttonInteractables)
        {
            interactable.OnClick.AddListener(() => activatedButton = interactable);
            interactable.OnClick.AddListener(() => activatedButtonGameObject = buttons[buttonInteractables.IndexOf(interactable)]);
            interactable.OnClick.AddListener(() => gameObject.GetComponent<ModeManager>().SetDrawingMode());
        }
    }

    public void CheckPointerUp()
    {
        pointerUp = true;
    }

    public void CheckPointerDown()
    {
        pointerUp = false;
    }

    // Activates solver (makes gameObject follow hand) when selectedComponent is an active gameObject
    IEnumerator ActivateSolverOnSelected()
    {
        yield return new WaitUntil(() => selectedComponent != null);
        if(selectedComponent.tag == "FreeDrawPoint" || selectedComponent.tag == "GrabPoint" || selectedComponent.tag == "EndGrabPoint")
        {
            selectedComponent.GetComponent<SolverHandler>().enabled = true;
        }
    }
}