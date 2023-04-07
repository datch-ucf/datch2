using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using Microsoft.MixedReality.Toolkit;

public class ClearAllButton : MonoBehaviour
{
    public GameObject mainTimeline;
    public GameObject menu;
    private GameObject[] drawingsArray;
    public GameObject pegMarkerGrid;
    public GameObject tapeMeasure;
    private GameObject[] emptyGridPointsArray;
    public GameObject toolsGenerator;
    private GameObject systemFileAccess;

    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.Find("Menu");
        pegMarkerGrid = GameObject.Find("PegGrid");
        tapeMeasure = GameObject.Find("TapeMeasure");
        toolsGenerator = GameObject.Find("ToolsGenerator");
        systemFileAccess = GameObject.Find("SystemFileAccess");
    }
    
    public void ClearAll()
    {

        // Destroy all drawings, drawing plane, and peg maker grid
        drawingsArray = GameObject.FindGameObjectsWithTag("Shape");

        foreach(GameObject drawing in drawingsArray)
        {
            Destroy(drawing);
        }

        Destroy(GameObject.FindGameObjectWithTag("DrawingPlane"));

        pegMarkerGrid.GetComponent<MarkerPegsManager>().ClearPegGrid();

        emptyGridPointsArray = GameObject.FindGameObjectsWithTag("EmptyGridPoint");
        foreach(GameObject emptyGridPoint in emptyGridPointsArray)
        {
            Destroy(emptyGridPoint);
        }

        foreach(GameObject measuringCube in toolsGenerator.GetComponent<ToolsGenerator>().generatedMeasuringCubes)
        {
            Destroy(measuringCube);
        }

        foreach (GameObject measuringTape in toolsGenerator.GetComponent<ToolsGenerator>().generatedMeasuringTapes)
        {
            Destroy(measuringTape);
        }

        foreach(GameObject imagePlane in systemFileAccess.GetComponent<SystemFileAccess>().generatedImagePlanes)
        {
            Destroy(imagePlane);
        }

        //tapeMeasure.GetComponent<TapeMeasureTool>().ToggleTapeMeasure(); // LATER: May need to allow multiple tape measures to be destroyed instead of toggling one on and off
    }

    //public void RestartScene()
    //{
    //    DontDestroyOnLoad(Camera.main.transform.GetChild(1).gameObject); // Don't destroy the pen
    //    ClearAllExemptions.timesReloaded++;
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}
}
