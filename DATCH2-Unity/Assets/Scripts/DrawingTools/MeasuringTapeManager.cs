using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using System.IO;

/// <summary>
/// Measuring distances (2 dimensions). Keeps track of measurement data on measuringTape
/// LOCATION: MeasuringTape prefab
/// </summary>


public class MeasuringTapeManager: MonoBehaviour
{
    private GameObject measureStartPoint, measureEndPoint, startPointMarker, endPointMarker;
    public LineRenderer measuringTape;
    public float measurement;
    private string measurementText;
    public Canvas measuringTapeCanvas;
    public TMP_Text measurementTextOutput;

    // Start is called before the first frame update
    void Start()
    {
        measureStartPoint = gameObject.transform.GetChild(1).gameObject;
        measureEndPoint = gameObject.transform.GetChild(2).gameObject;
        startPointMarker = gameObject.transform.GetChild(1).GetChild(0).gameObject;
        endPointMarker = gameObject.transform.GetChild(2).GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // Sets start and endpoints of line renderer at appropriate points on line
        measuringTape.SetPosition(0, measureStartPoint.gameObject.transform.position);
        measuringTape.SetPosition(1, measureEndPoint.gameObject.transform.position);

        measurementText = AdditionalUtilityFunctions.RoundToNearestHundredth(Vector3.Distance(measureStartPoint.transform.position, measureEndPoint.transform.position)).ToString();
        measurementTextOutput.SetText(measurementText + " m");
        measuringTapeCanvas.transform.position = new Vector3((measureStartPoint.transform.position.x + measureEndPoint.transform.position.x) / 2, (measureStartPoint.transform.position.y + measureEndPoint.transform.position.y) / 2 + 0.1f, (measureStartPoint.transform.position.z + measureEndPoint.transform.position.z) / 2); // Keep measurement centered on tape

        // Make markers always face user
        measureStartPoint.transform.rotation = Quaternion.LookRotation(measureStartPoint.transform.position - Camera.main.transform.position);
        measureEndPoint.transform.rotation = Quaternion.LookRotation(measureEndPoint.transform.position - Camera.main.transform.position);

        // Using formula to make measurements always face user
        measuringTapeCanvas.transform.rotation = Quaternion.LookRotation(measuringTapeCanvas.transform.position - Camera.main.transform.position); 
    }
}
