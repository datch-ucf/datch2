using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;


/// <summary>
/// DESCRIPTION: This script helps toggle pinning of the main menu
/// LOCATION: DATCH Logo Button
/// </summary>

public class PinMenuController : MonoBehaviour
{

    public GameObject menu;
    public SolverHandler menuSolverHandler;
    public GameObject menuContentParent;
    private Vector3 menuContentParentPosOffset;
    private Quaternion menuContentParentRotOffset;
    private bool firstPin = false;

    public void Start()
    {
        menu = GameObject.Find("Menu");
        if (firstPin == false)
        {
            menuContentParentPosOffset = menuContentParent.transform.localPosition;
            //menuContentParentRotOffset = Quaternion.Euler(0,0,0);
            menuContentParentRotOffset = menuContentParent.transform.localRotation;
            firstPin = true;
        }
    }
    public void TogglePinnedMode()
    {
        // Track menu to palm
        if (menu.GetComponent<HandConstraintPalmUp>().enabled == false)
        {
            UnpinMenu();
        }

        // Pin menu
        else if (menu.GetComponent<HandConstraintPalmUp>().enabled == true)
        {
            PinMenu();
        }
    }

    // Pin menu in space
    public void PinMenu()
    {
        menu.GetComponent<HandConstraintPalmUp>().enabled = false;
        menuContentParent.GetComponent<BoxCollider>().enabled = true;
        menuContentParent.GetComponent<ObjectManipulator>().enabled = true;
        menuContentParent.GetComponent<NearInteractionGrabbable>().enabled = true;
        menuContentParent.GetComponent<RotationAxisConstraint>().enabled = true;
        menu.transform.rotation = Quaternion.Euler(0, 0, 0);
        menuContentParent.transform.localRotation = Quaternion.Euler(0, 0, 0);
        menu.transform.position = new Vector3(menu.transform.position.x, menu.transform.position.y + 0.2f, menu.transform.position.z); // positions pinned menu slightly up
    }

    // Track menu to palm
    public void UnpinMenu()
    {
        menu.GetComponent<HandConstraintPalmUp>().enabled = true;
        menuContentParent.GetComponent<BoxCollider>().enabled = false;
        menuContentParent.GetComponent<ObjectManipulator>().enabled = false;
        menuContentParent.GetComponent<NearInteractionGrabbable>().enabled = false;
        menuContentParent.GetComponent<RotationAxisConstraint>().enabled = false;

        // Issue here where unpinning while in left-handed mode causes menu to be at incorrect offset. Fix later.
        menuContentParent.transform.localPosition = menuContentParentPosOffset;


        menuContentParent.transform.localRotation = menuContentParentRotOffset;          
    }
}