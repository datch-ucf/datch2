using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deletes shape within ShapeHolder prefab and maintains position of delete button
/// LOCATION: Delete button (on ShapeHolder prefab and DrawingPlane prefab)
/// </summary>
public class DeleteFunctions : MonoBehaviour
{
    private GameObject gestureContainer;

    void Start()
    {
        gestureContainer = Camera.main.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.parent.tag == "DrawingPlane")
        {
            // Position delete button at bottom center of drawing plane
            var planeCenter = gameObject.transform.parent.GetComponent<Renderer>().bounds.center;
            gameObject.transform.position = new Vector3(planeCenter.x, planeCenter.y - 0.3f, planeCenter.z);
            gameObject.transform.LookAt(Camera.main.transform.position);
        }

    }

    public void DeleteSelectedShape()
    {
        // Delete last clicked measuring tape (no onHover selection)
        if (gestureContainer.GetComponent<SelectionManager>().selectedComponent.CompareTag("MeasuringTape"))
        {
            // If the component's parent exists, the component is one of the points of the measuring tape, so delete the parent
            if (gestureContainer.GetComponent<SelectionManager>().selectedComponent.transform.parent != null)
            {
                Destroy(gestureContainer.GetComponent<SelectionManager>().selectedComponent.transform.parent.gameObject);
            }

            // If the component does not have a parent, it is the measuring tape itself, so it needs to be destroyed.
            else
            {
                Destroy(gestureContainer.GetComponent<SelectionManager>().selectedComponent);
            }
        }

        // Delete last clicked measuring cube (no OnHover selection)
        else if (gestureContainer.GetComponent<SelectionManager>().selectedComponent.CompareTag("MeasuringCube"))
        {
            // If the component's parent exists, the component is a child of the measuring cube, so delete the parent
            if (gestureContainer.GetComponent<SelectionManager>().selectedComponent.transform.parent != null)
            {
                Destroy(gestureContainer.GetComponent<SelectionManager>().selectedComponent.transform.parent.gameObject);
            }

            // If the component does not have a parent, it is the measuring cube itself, so it needs to be destroyed.
            else
            {
                Destroy(gestureContainer.GetComponent<SelectionManager>().selectedComponent);
            }
        }

        // Delete last clicked measuring cube (no OnHover selection) if a handle is last component grabbed
        else if (gestureContainer.GetComponent<SelectionManager>().selectedComponent.transform.parent.name.Contains("rigRoot") && gestureContainer.GetComponent<SelectionManager>().selectedComponent.transform.parent.parent.CompareTag("MeasuringCube"))
        {
            Destroy(gestureContainer.GetComponent<SelectionManager>().selectedComponent.transform.parent.parent.gameObject);
        }

        // Delete last clicked image plane (no onHover selection)
        else if (gestureContainer.GetComponent<SelectionManager>().selectedComponent.CompareTag("ImagePlane"))
        {
            Destroy(gestureContainer.GetComponent<SelectionManager>().selectedComponent);
        }

        // Delete selected shape
        else
        {
            StartCoroutine(WaitToUpdateShapesArray());
            Destroy(gestureContainer.GetComponent<SelectionManager>().selectedShape);
            StartCoroutine(WaitToUpdateShapesArray());
        }
    }

    IEnumerator WaitToUpdateShapesArray()
    {
        yield return new WaitForSeconds(0.1f);
        gestureContainer.GetComponent<SelectionManager>().UpdateShapesArray();
    }

}
