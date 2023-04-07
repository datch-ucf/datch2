using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Show the distance between the start and end of the tape so that objects can be measured.
/// LOCATION: TapeMeasure prefab
/// </summary>
public class TapeMeasureTool : MonoBehaviour
{
    public GameObject measureStartPoint;
    public GameObject measureEndPoint;
    public LineRenderer measuringTape;
    public float measurement;
    public TextMeshPro measurementText;

    // Start is called before the first frame update
    void Start()
    {
        //measuringTape = gameObject.GetComponent<LineRenderer>();
        measurementText = gameObject.GetComponentInChildren<TextMeshPro>();

    }

    // Update is called once per frame
    void Update()
    {
        measuringTape.SetPosition(0, measureStartPoint.gameObject.transform.position);
        measuringTape.SetPosition(1, measureEndPoint.gameObject.transform.position);
        
        measurement = Vector3.Distance(measureStartPoint.transform.position, measureEndPoint.transform.position);
        measurement = (float)System.Math.Round(measurement, 3); // Round to nearest 3 decimal places 
        measurementText.SetText(measurement.ToString() + " m");

        //measurementText.transform.localPosition = new Vector3((measureStartPoint.transform.position.x + measureEndPoint.transform.position.x)/2 + 0.005f, (measureStartPoint.transform.position.y + measureEndPoint.transform.position.y)/2, (measureStartPoint.transform.position.z + measureEndPoint.transform.position.z)/2); // Keep measurement centered on tape
        measurementText.transform.localPosition = new Vector3((measureStartPoint.transform.position.x + measureEndPoint.transform.position.x)/2 + 0.005f, (measureStartPoint.transform.position.y + measureEndPoint.transform.position.y)/2, (measureStartPoint.transform.position.z + measureEndPoint.transform.position.z)/2); // Keep measurement centered on tape
        measurementText.transform.LookAt(Camera.main.transform);
    }

    public void ToggleTapeMeasure()
    {
        gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.25f;

        if(measuringTape.enabled == false)
        {
            measuringTape.enabled = true;
            gameObject.SetActive(true);
        }
        else
        {
            measuringTape.enabled = false;
            gameObject.SetActive(false);
        }
    }

    //public void PositionInFront()
    //{
    //    gameObject.SetActive(true);
    //    gameObject.transform.position = new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, Camera.main.transform.localPosition.z + 0.25f);
    //}

    //public void PositionTapeMeasure()
    //{
    //    print("Positioning");
    //    // Only need one drawing plane to be present in scene at a time
    //    // When newSurface button is pressed, find any existing drawing plane and destroy it, then generate a new one in front of user
    //    //var existingDrawingPlane = GameObject.FindGameObjectWithTag("DrawingPlane");

    //    //drawingPlane = Instantiate(drawingPlanePrefab, Camera.main.transform.position + Camera.main.transform.forward * 0.5f, Quaternion.identity); // generate an instance of the object in front of camera
    //    // Positions drawing plane in front of user and perpendicular to floor
    //    //drawingPlane.transform.LookAt(Camera.main.transform);
    //    //drawingPlane.transform.Rotate(0.0f, 90.0f, 90.0f);
    //    gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
    //    //drawingPlane.transform.position = Camera.main.transform.localPosition + Camera.main.transform.forward * 1.0f; // Pushes drawing plane forward to just in front of the user
    //}
}
