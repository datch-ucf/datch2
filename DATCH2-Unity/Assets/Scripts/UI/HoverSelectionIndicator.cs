using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;

/// <summary>
/// Works with HoverSelectionManager.cs to indicate user is selecting a shape.
/// LOCATION: FocusSelectionIndicator gameObject
/// </summary>


public class HoverSelectionIndicator : MonoBehaviour
{
    public GameObject shapePrefab;
    public Image selectionTimer;
    public float remainingTime, totalTime;
    public bool fillingSelectionTimer;
    private GameObject gestureContainer;
    //public GameObject leftHand;
    //public GameObject rightHand;
    

    public Material customHandMeshOutlineMat;  // Material that shows in editor
    public Material customArticulatedHandMeshMat;  // Material that shows in HMD/Headset

    private GameObject menu;
    public GameObject dominantSystemHand;
    public GameObject dominantRiggedHand;
    private string dominantSystemHandRef;
    private string dominantRiggedHandRef;


    void Start()
    {
        menu = GameObject.Find("Menu");
        gestureContainer = Camera.main.transform.GetChild(0).gameObject;
        totalTime = shapePrefab.GetComponent<HoverSelectionManager>().hoverSelectDelay;
        selectionTimer.fillAmount = 0;
        gameObject.GetComponent<SolverHandler>().TrackedHandedness = menu.GetComponent<MenuOrientation>().dominantHand;

        // User can ONLY SELECT USING DOMINANT HAND
        // Find dominant hand
        if (menu.GetComponent<MenuOrientation>().dominantHand == Handedness.Right)
        {
            dominantSystemHandRef = "R_Hand";
            dominantRiggedHandRef = "R_Hand_MRTK_Rig";
        }

        if (menu.GetComponent<MenuOrientation>().dominantHand == Handedness.Left)
        {
            dominantSystemHandRef = "L_Hand";
            dominantRiggedHandRef = "L_Hand_MRTK_Rig";
        }


    }

    void Update()
    {
        // Locate dominant hand and set its material to the custom hand mesh material
        if (dominantSystemHand == null) // if dominant hand is null
        {
            dominantSystemHand = GameObject.Find(dominantSystemHandRef); // look for it
            dominantRiggedHand = GameObject.Find(dominantRiggedHandRef); // look for it

            if (dominantSystemHand != null) // if dominant hand is found
            {

                if (dominantSystemHand.GetComponent<Renderer>().material != customHandMeshOutlineMat) // if dominant hand material is net set properly
                {
                    dominantSystemHand.GetComponent<Renderer>().material = customHandMeshOutlineMat; // set to custom material
                }
            }

            if (dominantRiggedHand != null) // if dominant hand is found
            {
                if (dominantRiggedHand.GetComponent<Renderer>() != null) // Do not proceed with changing material if app can't find Renderer component (happens when running in editor)
                {
                    if (dominantRiggedHand.GetComponent<Renderer>().material != customArticulatedHandMeshMat) // if dominant hand material is net set properly
                    {
                        dominantRiggedHand.GetComponent<Renderer>().material = customArticulatedHandMeshMat; // set to custom material
                    }
                }

            }

        }

        // Controls filling selection timer circle
        if (fillingSelectionTimer == true && (gestureContainer.GetComponent<SelectionManager>().pointerUp == true || gestureContainer.GetComponent<GestureDetector>().isPinching == false))
        {
            StartSelectionTimer(1.5f);
        }
        else
        {
            // Change to original color if selection timer runs down to zero or user stops hovering over selected shape
            selectionTimer.fillAmount = 0;

            customHandMeshOutlineMat.SetColor("_Color", new Color32(105, 111, 118, 255)); // Original color
            customArticulatedHandMeshMat.SetColor("_Fill_Color_", new Color32(81, 81, 81, 255)); // Original color

            if (dominantSystemHand != null)
            {
                dominantSystemHand.GetComponent<Renderer>().material.SetColor("_Color", new Color32(105, 111, 118, 255));
            }

            if (dominantRiggedHand != null) // Do not proceed with changing material if app can't find Renderer component (happens when running in editor)
            {
                if (dominantRiggedHand.GetComponent<Renderer>() != null)
                {
                    dominantRiggedHand.GetComponent<Renderer>().material.SetColor("_Fill_Color_", new Color32(81, 81, 81, 255));
                }
            }

        }

        //leftHand = GameObject.Find("L_Hand");
        //rightHand = GameObject.Find("R_Hand");
    }

    public void StartSelectionTimer(float totalTime)
    {
        if (selectionTimer.fillAmount < totalTime)
        {
            selectionTimer.fillAmount += 1.0f / totalTime * Time.deltaTime; // Fill timer until amount of time required to select has elapsed

            // Change hand color to blue if user has just selected a shape
            if (selectionTimer.fillAmount >= 1) // Do this if selectionTimer is 100% filled
            {
                selectionTimer.fillAmount = 1.0f;
                customHandMeshOutlineMat.SetColor("_Color", new Color32(0, 130, 255, 255)); // Highlight color
                customArticulatedHandMeshMat.SetColor("_Fill_Color_", new Color32(0, 130, 255, 255)); // Highlight color

                if (dominantSystemHand != null)
                {
                    dominantSystemHand.GetComponent<Renderer>().material.SetColor("_Color", new Color32(0, 130, 255, 255)); // Highlight color
                }

                if (dominantRiggedHand != null)
                {
                    if (dominantRiggedHand.GetComponent<Renderer>() != null)  // Do not proceed with changing material if app can't find Renderer component (happens when running in editor)
                    {
                        dominantRiggedHand.GetComponent<Renderer>().material.SetColor("_Fill_Color_", new Color32(0, 130, 255, 255)); // Highlight color
                    }
                }

            }

        }
    }



}
