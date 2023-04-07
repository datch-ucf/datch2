using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows user to align points within a shape by indicating with a line between the points and snapping in place.
/// LOCATION: PointsAligner (child on Shape prefab)
/// </summary>

public class PointsAlignmentHelper : MonoBehaviour
{
    public GameObject selectedShape;
    public GameObject selectedComponent;
    public GameObject alignedPoint;
    public GameObject gestureContainer;
    public List<Transform> selectedShapePoints = new List<Transform>();
    bool shapePointsFound = false;
    public GameObject xAlignmentLine;
    public GameObject yAlignmentLine;
    public GameObject zAlignmentLine;



    // Start is called before the first frame update
    void Start()
    {
        gestureContainer = Camera.main.transform.GetChild(0).gameObject;
        //xAlignmentLine.SetActive(false);
        //yAlignmentLine.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        selectedShape = gestureContainer.GetComponent<SelectionManager>().selectedShape;
        selectedComponent = gestureContainer.GetComponent<SelectionManager>().selectedComponent;

        

        // Find each of the other points in the selected shape (excluding the currently selected point (selectedComponent)
        selectedShapePoints.Clear();
        
        // Line
        if(selectedShape.transform.GetChild(1).CompareTag("GrabPoint"))
        {
            foreach (Transform child in selectedShape.transform)
            {
                if (child.CompareTag("GrabPoint") || child.CompareTag("EndGrabPoint") && child != selectedComponent) 
                {
                    selectedShapePoints.Add(child);
                }
            }
            selectedShapePoints.RemoveAt(0);
        }
        
        // Curve
        else if (selectedShape.transform.GetChild(1).CompareTag("Curve"))
        {
            foreach (Transform child in selectedShape.transform.GetChild(1))
            {
                if (child.CompareTag("EndGrabPoint") && child != selectedComponent) // Only working with end grab points. May reconsider allowing for all later 
                {
                    selectedShapePoints.Add(child);
                }
                
            }
        }

        // Check that grabbed point is within x distance to another point in the shape on one axis
        for (int i = 0; i < selectedShapePoints.Count; i++)
        {
            // Check y-axis alignment
            if ((selectedComponent.transform.position.x >= selectedShapePoints[i].transform.position.x - 0.05f && selectedComponent.transform.position.x <= selectedShapePoints[i].transform.position.x + 0.05f) && selectedComponent.transform != selectedShapePoints[i] && selectedShapePoints.Count > 1) // If grabbed point x is close enough to x of another point...they are aligned
            {
                // Draw a line
                yAlignmentLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(selectedShapePoints[i].transform.position.x, selectedShapePoints[i].transform.position.y - 2.0f, selectedShapePoints[i].transform.position.z));
                yAlignmentLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(selectedShapePoints[i].transform.position.x, selectedShapePoints[i].transform.position.y, selectedShapePoints[i].transform.position.z));
                yAlignmentLine.GetComponent<LineRenderer>().SetPosition(2, new Vector3(selectedShapePoints[i].transform.position.x, selectedShapePoints[i].transform.position.y + 2.0f, selectedShapePoints[i].transform.position.z));

                yAlignmentLine.SetActive(true); // Points are aligned on y-axis
                
                // Snapping to alignment line (only if user intentionally moves point closer to alignment line)
                if (selectedComponent.transform.position.x >= selectedShapePoints[i].transform.position.x - 0.025f && selectedComponent.transform.position.x <= selectedShapePoints[i].transform.position.x + 0.025f)
                {
                    selectedComponent.transform.position = new Vector3(selectedShapePoints[i].transform.position.x, selectedComponent.transform.position.y, selectedComponent.transform.position.z);
                }
                    

            }
            else if (selectedComponent.transform.position.x <= selectedShapePoints[i].transform.position.x - 0.03f || selectedComponent.transform.position.x >= selectedShapePoints[i].transform.position.x + 0.03f)
            {
                yAlignmentLine.SetActive(false); // Points are not aligned along y-axis
            }

            // Check x-axis alignment
            if ((selectedComponent.transform.position.y >= selectedShapePoints[i].transform.position.y - 0.03f && selectedComponent.transform.position.y <= selectedShapePoints[i].transform.position.y + 0.03f) && selectedComponent.transform != selectedShapePoints[i] && selectedShapePoints.Count > 1) // If grabbed point x is close enough to x of another point...they are aligned
            {
                // Draw a line
                xAlignmentLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(selectedShapePoints[i].transform.position.x - 2.0f, selectedShapePoints[i].transform.position.y, selectedShapePoints[i].transform.position.z));
                xAlignmentLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(selectedShapePoints[i].transform.position.x, selectedShapePoints[i].transform.position.y, selectedShapePoints[i].transform.position.z));
                xAlignmentLine.GetComponent<LineRenderer>().SetPosition(2, new Vector3(selectedShapePoints[i].transform.position.x + 2.0f, selectedShapePoints[i].transform.position.y, selectedShapePoints[i].transform.position.z));

                xAlignmentLine.SetActive(true); // Points are aligned on y-axis

                // Snapping to alignment line (only if user intentionally moves point closer to alignment line)
                if (selectedComponent.transform.position.y >= selectedShapePoints[i].transform.position.y - 0.03f && selectedComponent.transform.position.y <= selectedShapePoints[i].transform.position.y + 0.03f)
                {
                    selectedComponent.transform.position = new Vector3(selectedComponent.transform.position.x, selectedShapePoints[i].transform.position.y, selectedComponent.transform.position.z);
                }

            }
            else if (selectedComponent.transform.position.y <= selectedShapePoints[i].transform.position.y - 0.03f || selectedComponent.transform.position.y >= selectedShapePoints[i].transform.position.y + 0.03f)
            {
                xAlignmentLine.SetActive(false); // Points are not aligned along y-axis
            }





            //if (selectedComponent.transform.position.y >= selectedShapePoints[i].transform.position.y - 0.05f && selectedComponent.transform.position.y <= selectedShapePoints[i].transform.position.y + 0.05f) // If grabbed point x is close enough to x of another point...they are aligned
            //{
            //    // Draw a line
            //    xAlignmentLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(selectedShapePoints[i].transform.position.x - 2.0f, selectedShapePoints[i].transform.position.y, selectedShapePoints[i].transform.position.z ));
            //    xAlignmentLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(selectedShapePoints[i].transform.position.x, selectedShapePoints[i].transform.position.y, selectedShapePoints[i].transform.position.z));
            //    xAlignmentLine.GetComponent<LineRenderer>().SetPosition(2, new Vector3(selectedShapePoints[i].transform.position.x + 2.0f, selectedShapePoints[i].transform.position.y, selectedShapePoints[i].transform.position.z ));

            //    if (selectedShapePoints.Count > 1)
            //    {
            //        //xAlignmentLine.SetActive(true); // Points are aligned on x-axis
            //        print("Aligned on y");


            //    }

            //}
            //else
            //{

            //    xAlignmentLine.SetActive(false); // Points are not aligned on x-axis
            //}

            //// Check z-axis alignment
            //if (selectedComponent.transform.position.z >= selectedShapePoints[i].transform.position.z - 0.05f && selectedComponent.transform.position.z <= selectedShapePoints[i].transform.position.z + 0.05f) // If grabbed point x is close enough to x of another point...they are aligned
            //{
            //    // Draw a line
            //    zAlignmentLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(selectedShapePoints[i].transform.position.x, selectedShapePoints[i].transform.position.y, selectedShapePoints[i].transform.position.z - 2.0f));
            //    zAlignmentLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(selectedShapePoints[i].transform.position.x, selectedShapePoints[i].transform.position.y, selectedShapePoints[i].transform.position.z));
            //    zAlignmentLine.GetComponent<LineRenderer>().SetPosition(2, new Vector3(selectedShapePoints[i].transform.position.x, selectedShapePoints[i].transform.position.y, selectedShapePoints[i].transform.position.z + 2.0f));

            //    zAlignmentLine.SetActive(true); // Points are aligned on z-axis
            //}
            //else
            //{

            //    zAlignmentLine.SetActive(false); // Points are not aligned on z-axis
            //}
        }
    }
}
