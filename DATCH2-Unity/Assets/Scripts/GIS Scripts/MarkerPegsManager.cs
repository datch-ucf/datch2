using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using Microsoft.MixedReality.Toolkit.SpatialAwareness; // added
using Microsoft.MixedReality.Toolkit; // added



/* 
* Controls the basic functionality involving the grid used for marking at sites. 
* SCRIPT LOCATION: Grid empty game object with Grid Object Collection Component
*/

public class MarkerPegsManager : MonoBehaviour
{
    private GameObject menu;

    // MainPegGrid Variables
    public int numCols = 20, numRows = 20, numPegs = 400;
    public float colSpacing, rowSpacing;
    public GameObject peg;
    public GameObject[] pegsArr;
    public GameObject colSpacingSlider, rowSpacingSlider;
    public TMP_Text colSpacingSliderText, rowSpacingSliderText, numRowsLabel, numColsLabel;
    public List<GameObject> spatialMeshList = new List<GameObject>();
    public bool gridIsPlaced = false;
    public float pegGridParentStartRot;
    public GameObject[] activePegMarkerGrids;

    // miniPegGrid Variables
    public GameObject miniPegGrid, miniPeg;
    public float clampXMin = -0.07f, clampXMax = 0.07f, clampYMin = -0.07f, clampYMax = 0.07f, clampZMin = 0.0f, clampZMax = 0.0f;
    public float miniPegGridScale = 1;

    //MiniGrid d-pad
    public int dPadNumRows,dPadNumCols, dPadMarkersTotal;
    public GameObject dPadMarker, dPad;
    //public GameObject[] dPadArr;



    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.Find("Menu");
        miniPegGrid = menu.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(2).GetChild(0).GetChild(1).GetChild(3).GetChild(0).gameObject;
        colSpacingSlider = menu.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(2).GetChild(2).gameObject;
        rowSpacingSlider = menu.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(2).GetChild(1).gameObject;
        colSpacingSliderText = colSpacingSlider.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TMP_Text>();
        rowSpacingSliderText = rowSpacingSlider.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TMP_Text>();
        numColsLabel = menu.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(2).GetChild(3).GetChild(0).GetChild(2).gameObject.GetComponent<TMP_Text>();
        numRowsLabel = menu.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(2).GetChild(3).GetChild(0).GetChild(1).gameObject.GetComponent<TMP_Text>();
        dPad = menu.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(2).GetChild(0).GetChild(1).GetChild(2).gameObject;

        //// Set up dPad for miniPegMarkerGrid (Now just copied and pasted under dPad GameObject. Delete and uncomment this to change settings and create on Start instead)
        //dPad.GetComponent<GridObjectCollection>().Rows = dPadNumRows;
        //dPadMarkersTotal = dPadNumRows * dPadNumCols;

        //// Create pegs for main grid
        //dPadArr = new GameObject[dPadMarkersTotal];
        //for (int i = 0; i < dPadMarkersTotal; i++)
        //{
        //    dPadArr[i] = Instantiate(dPadMarker, Vector3.zero, Quaternion.identity);
        //    dPadArr[i].transform.parent = dPad.transform;
        //    dPadArr[i].transform.localRotation = Quaternion.Euler(0, 0, 0); // Rotates each peg to stick up from ground
        //    dPadArr[i].transform.localScale = new Vector3(0.02f, 0.02f, 0.025f);
        //}

        //dPad.GetComponent<GridObjectCollection>().UpdateCollection();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<GridObjectCollection>().Rows = numRows;
        numPegs = numRows * numCols;

        gameObject.GetComponent<GridObjectCollection>().CellWidth = colSpacing;
        gameObject.GetComponent<GridObjectCollection>().CellHeight = rowSpacing;

        // Update any changes to the grid parameters
        gameObject.GetComponent<GridObjectCollection>().UpdateCollection();

        // ***ISSUE: Rotating mini too much causes mainPegGrid to move in opposite direction FIX LATER (REMOVED FEATURE FOR NOW)
        //gameObject.transform.parent.transform.localRotation = Quaternion.Euler(gameObject.transform.parent.transform.localRotation.x, miniPegGrid.transform.parent.transform.localRotation.z * -20 + Camera.main.transform.localEulerAngles.y, gameObject.transform.parent.transform.localRotation.z); // Multiplied by 20 for scaling (miniGrid rotation is very small in comparison to real world mainPegGrid rotation)          
        //^^ THIS IS CAUSING THE ENTIRE THING TO MOVE WITH HEAD. CHECK IT.

        //gameObject.transform.parent.transform.localRotation = Quaternion.Euler(gameObject.transform.parent.transform.localRotation.x, miniPegGrid.transform.parent.transform.localRotation.z * -20 + pegGridParentStartRot.y, gameObject.transform.parent.transform.localRotation.z); // Multiplied by 20 for scaling (miniGrid rotation is very small in comparison to real world mainPegGrid rotation)          
        // ^^INSTEAD, ADD A VARIABLE THAT HOLDS WHAT THE POSITION AT START IS.

        // Set pegs on floor
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, Camera.main.GetComponent<FloorFinder>().floorLvl, gameObject.transform.position.z);

        // Limit miniPegGrid movement to plane
        var miniPegGridPos = miniPegGrid.transform.parent.localPosition;

        // If the miniPegGrid is moved outside of bounds, push it back to within the bounds. (This essentially clamps movement)
        if (miniPegGrid.transform.parent.localPosition.x < clampXMin)
        {
            miniPegGridPos.x = clampXMin + 0.0001f;
        }

        if (miniPegGrid.transform.parent.localPosition.x > clampXMax)
        {
            miniPegGridPos.x = clampXMax - 0.0001f;
        }

        if (miniPegGrid.transform.parent.localPosition.y < clampYMin)
        {
            miniPegGridPos.y = clampYMin + 0.0001f;
        }

        if (miniPegGrid.transform.parent.localPosition.y > clampYMax)
        {
            miniPegGridPos.y = clampYMax - 0.0001f;
        }

        miniPegGridPos.z = miniPegGrid.transform.parent.localPosition.z;

        miniPegGrid.transform.parent.localPosition = miniPegGridPos;
    }

    public void IncreaseNumRows()
    {
        numRows += 10;

        // Clear array to prepare for changing size
        foreach (GameObject p in pegsArr)
        {
            Destroy(p);
        }

        numPegs = numRows * numCols;
        CreatePegs();
    }

    public void DecreaseNumRows()
    {
        numRows -= 10;

        // Clear array to prepare for changing size
        foreach (GameObject p in pegsArr)
        {
            Destroy(p);
        }

        numPegs = numRows * numCols;
        CreatePegs();
    }

    public void IncreaseNumCols()
    {
        numCols += 10;

        // Clear array to prepare for changing size
        foreach (GameObject p in pegsArr)
        {
            Destroy(p);
        }

        numPegs = numRows * numCols;
        CreatePegs();

    }

    public void DecreaseNumCols()
    {
        numCols -= 10;

        // Clear array to prepare for changing size
        foreach (GameObject p in pegsArr)
        {
            Destroy(p);
        }

        numPegs = numRows * numCols;
        CreatePegs();
    }

    public void CreatePegs()
    {
        // Clear current pegs from pegsArr in scene (prevent extra pegs from being created)
        foreach (GameObject p in pegsArr)
        {
            GameObject.Destroy(p);
        }

        // Create pegs for main grid
        pegsArr = new GameObject[numPegs];
        for (int i = 0; i < numPegs; i++)
        {
            pegsArr[i] = Instantiate(peg, Vector3.zero, Quaternion.identity);
            pegsArr[i].transform.parent = gameObject.transform;
            pegsArr[i].transform.localRotation = Quaternion.Euler(90, 0, 0); // Rotates each peg to stick up from ground
        }
        
        // Update grid menu text
        numRowsLabel.text = numRows.ToString();
        numColsLabel.text = numCols.ToString();

        // Make pegMarkers face same direction as user when initially created
        gameObject.transform.parent.transform.localRotation = Quaternion.Euler(gameObject.transform.parent.transform.localRotation.x, Camera.main.transform.localEulerAngles.y, gameObject.transform.parent.transform.localRotation.z); // original 
        
        //pegGridParentStartRot = Camera.main.transform.localEulerAngles.y; // Rotation of pegMarkers when created (For use by moving miniPegs)
        
        // Change miniPegGrid size to reflect shape of pegMarkerGrid
        if (numRows > numCols && numRows > 20)
        {
            miniPegGrid.transform.localScale = new Vector3(0.5f, 1, 1);
        }
        if (numRows > numCols && numRows < 20)
        {
            miniPegGrid.transform.localScale = new Vector3(1.5f, 1, 1);
        }
        if (numRows < numCols && numRows > 20)
        {
            miniPegGrid.transform.localScale = new Vector3(1, 0.5f, 1);
        }
        if (numRows < numCols && numRows < 20)
        {
            miniPegGrid.transform.localScale = new Vector3(1, 1.5f, 1);
        }
        if (numRows == numCols)
        {
            miniPegGrid.transform.localScale = new Vector3(1, 1, 1);
        }

    }

    public void UpdateColSpacing()
    {
        colSpacing = (float)System.Math.Round(AdditionalUtilityFunctions.Remap(colSpacingSlider.GetComponent<PinchSlider>().SliderValue, 0.0f, 1.0f, 0.0f, 2.0f), 2); // Remap peg spacing to cover smaller area
        colSpacingSliderText.text = colSpacing.ToString() + " m"; // Updates the slider UI values with an arbitrary number and icon that changes in size
    }

    public void UpdateRowSpacing()
    {
        rowSpacing = (float)System.Math.Round(AdditionalUtilityFunctions.Remap(rowSpacingSlider.GetComponent<PinchSlider>().SliderValue, 0.0f, 1.0f, 0.0f, 2.0f), 2); // Remap peg spacing to cover smaller area

        // Updates the slider UI values with an arbitrary number and icon that changes in size
        rowSpacingSliderText.text = rowSpacing.ToString() + " m";
    }

    // Clear pegGrid by resetting values to original (Runs in ClearAllButton script)
    public void ClearPegGrid()
    {
        // Original pegMarkerGrid values
        numCols = 20;
        numRows = 20;
        colSpacing = 1;
        rowSpacing = 1;
        numPegs = numRows * numCols;

        // Clear arrays
        foreach (GameObject p in pegsArr)
        {
            Destroy(p);
        }

        numPegs = numRows * numCols;

        // Restart spacing sliders and precision
        colSpacing = colSpacingSlider.GetComponent<PinchSlider>().SliderValue = 0.5f;
        rowSpacing = rowSpacingSlider.GetComponent<PinchSlider>().SliderValue = 0.5f;
        UpdateRowSpacing();
        UpdateColSpacing();

        miniPegGrid.transform.localPosition = new Vector3(0, 0, 0);
        //miniPegGridScale = 1;
        //miniPegGridScale = Mathf.Clamp(miniPegGridScale, 0.01f, 100f);
        //miniPegGridScale = (float)System.Math.Round(miniPegGridScale, 2);
        //precisionLvlText.text = miniPegGridScale.ToString() + " m";

        //gameObject.GetComponent<GridObjectCollection>().UpdateCollection();

        //gameObject.GetComponent<GridObjectCollection>().Rows = numRows;

    }


    //public void PlacePegMarkersToggle()
    //{
    //    if(gridIsPlaced == true)
    //    {
    //        miniPegGrid.GetComponent<ObjectManipulator>().enabled = true; // Allow movement
    //        gridIsPlaced = false;
    //    }
    //    if(gridIsPlaced == false)
    //    {
    //        miniPegGrid.GetComponent<ObjectManipulator>().enabled = false; // Stop movement
    //        gridIsPlaced = true;
    //    }

    //}    

    public void IncrPegsRot()
    {
        gameObject.transform.parent.localRotation =  Quaternion.Euler(gameObject.transform.parent.localEulerAngles.x, gameObject.transform.parent.localEulerAngles.y + 10, gameObject.transform.parent.localEulerAngles.z);
    }

    public void DecrPegsRot()
    {
        gameObject.transform.parent.localRotation = Quaternion.Euler(gameObject.transform.parent.localEulerAngles.x, gameObject.transform.parent.localEulerAngles.y - 10, gameObject.transform.parent.localEulerAngles.z);
    }

    public void TogglePegMarkers()
    {
        if (gameObject.activeInHierarchy == true)
        {
            gameObject.SetActive(false);
            //miniPegGrid.SetActive(false);
        }
        else if (gameObject.activeInHierarchy == false)
        {
            gameObject.SetActive(true);
            //miniPegGrid.SetActive(true);
        }
    }
}
