using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Measuring dimensions. Keeps track of measurement data on measuringCube 
/// LOCATION: MeasuringCube prefab 
/// </summary>

public class MeasuringCubeManager : MonoBehaviour
{
    public Canvas measuringCubeCanvas;
    public TMP_Text measurementTextOutput;
    private string measTextX, measTextY, measTextZ;
    private Vector3 measuringCubeCanvasSize;

    // Start is called before the first frame update
    void Start()
    {

        measurementTextOutput = gameObject.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        //measuringCubeCanvasSize = measuringCubeCanvas.transform.localScale;
        //measuringCubeCanvasSize = measuringCubeCanvas.GetComponent<RectTransform>().localScale;
    }

    // Update is called once per frame
    void Update()
    {
        measTextX = AdditionalUtilityFunctions.RoundToNearestHundredth(gameObject.transform.localScale.x).ToString();
        measTextY = AdditionalUtilityFunctions.RoundToNearestHundredth(gameObject.transform.localScale.y).ToString();
        measTextZ = AdditionalUtilityFunctions.RoundToNearestHundredth(gameObject.transform.localScale.z).ToString();

        measurementTextOutput.SetText("x: " + measTextX + "m \n" + "y: " + measTextY + "m \n" + "z: " + measTextZ + "m");
    }
}
