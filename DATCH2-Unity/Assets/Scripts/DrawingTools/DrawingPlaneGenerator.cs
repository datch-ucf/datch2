using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingPlaneGenerator : MonoBehaviour
{
    public GameObject drawingPlanePrefab;
    GameObject drawingPlane;
    public GameObject drawOnPlaneCheckbox;

    public void InstantiateAndPosition()
    {
        // Only need one drawing plane to be present in scene at a time
        // When newSurface button is pressed, find any existing drawing plane and destroy it, then generate a new one in front of user
        //var existingDrawingPlane = GameObject.FindGameObjectWithTag("DrawingPlane");
        if (drawingPlane != null)
        {
            Destroy(drawingPlane);
        }

        drawingPlane = Instantiate(drawingPlanePrefab, Camera.main.transform.position + Camera.main.transform.forward * 0.75f, Quaternion.identity); // generate an instance of the object in front of camera
        // Positions drawing plane in front of user and perpendicular to floor
        drawingPlane.transform.LookAt(Camera.main.transform); // drawing plane faces camera
        drawingPlane.transform.localEulerAngles = new Vector3(0.0f, drawingPlane.transform.localEulerAngles.y, 0.0f); // sets drawing plane perpendicular to floor
        drawingPlane.transform.position = Camera.main.transform.localPosition + Camera.main.transform.forward * 0.75f; // Pushes drawing plane forward to just in front of the user
        drawingPlane.transform.position = new Vector3(drawingPlane.transform.position.x, Camera.main.transform.position.y, drawingPlane.transform.position.z); // reposition height of plane based on camera's y position (as opposed to fwd vector)


    }

    public void ShowHideDrawingPlane()
    {
        if (drawingPlane != null && drawingPlane.activeInHierarchy == true)
        {
            drawingPlane.SetActive(false);
        }
        else if (drawingPlane != null && drawingPlane.activeInHierarchy == false)
        {
            InstantiateAndPosition();

            // Positions drawing plane in front of user and perpendicular to floor
            drawingPlane.transform.SetPositionAndRotation(Camera.main.transform.position + Camera.main.transform.forward * 0.75f, Quaternion.identity);
            drawingPlane.transform.LookAt(Camera.main.transform); // drawing plane faces camera
            drawingPlane.transform.localEulerAngles = new Vector3(0.0f, drawingPlane.transform.localEulerAngles.y, 0.0f); // sets drawing plane perpendicular to floor
            drawingPlane.transform.position = Camera.main.transform.localPosition + Camera.main.transform.forward * 0.75f; // Pushes drawing plane forward to just in front of the user
            drawingPlane.transform.position = new Vector3(drawingPlane.transform.position.x, Camera.main.transform.position.y, drawingPlane.transform.position.z); // reposition height of plane based on camera's y position (as opposed to fwd vector)
            drawingPlane.SetActive(true);
        }
    }

    public void CreateOrToggleDrawingPlane()
    {
        if(drawingPlane == null)
        {
            InstantiateAndPosition();
        }
        else if(drawingPlane != null)
        {
            ShowHideDrawingPlane();
        }
    }
}
