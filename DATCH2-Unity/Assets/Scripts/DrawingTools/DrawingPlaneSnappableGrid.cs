using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Creates a 10x10 snappable grid for the drawing plane
/// LOCATION: SnappableDrawingPlane prefab
/// </summary>

public class DrawingPlaneSnappableGrid : MonoBehaviour
{
    public GameObject[] gridPointsArr;
    public GameObject gridPoint;
    private int numGridPoints;
    public GameObject col1Startpoint, col2Startpoint, col3Startpoint, col4Startpoint, col5Startpoint, col6Startpoint, col7Startpoint, col8Startpoint, col9Startpoint, col10Startpoint;
    public GameObject col1Endpoint, col2Endpoint, col3Endpoint, col4Endpoint, col5Endpoint, col6Endpoint, col7Endpoint, col8Endpoint, col9Endpoint, col10Endpoint;
    public GameObject row1Startpoint, row2Startpoint, row3Startpoint, row4Startpoint, row5Startpoint, row6Startpoint, row7Startpoint, row8Startpoint, row9Startpoint, row10Startpoint;
    public GameObject row1Endpoint, row2Endpoint, row3Endpoint, row4Endpoint, row5Endpoint, row6Endpoint, row7Endpoint, row8Endpoint, row9Endpoint, row10Endpoint;
    public GameObject empty; // This empty will hold the position of column 1 and column 10 (used to prevent line renderer error)

    // Start is called before the first frame update
    void Start()
    {
        // Create the intersection points of the grid. Total number of points will result in a square 10x10 grid
        numGridPoints = gameObject.GetComponent<GridObjectCollection>().Rows * gameObject.GetComponent<GridObjectCollection>().Rows; // Squaring the number of rows (i.e. creates a 5x5 grid)


        // Create pegs for main grid
        gridPointsArr = new GameObject[numGridPoints];
        for (int i = 0; i < numGridPoints; i++)
        {
            gridPointsArr[i] = Instantiate(gridPoint, Vector3.zero, Quaternion.identity, gameObject.transform);
        }
        gameObject.GetComponent<GridObjectCollection>().UpdateCollection();

        // Set specific gridPoints as startpoints and endpoints of grid lines
        // First row
        row1Startpoint = gameObject.transform.GetChild(0).gameObject;
        row1Endpoint = gameObject.transform.GetChild(90).gameObject;
        // 2nd row
        row2Startpoint = gameObject.transform.GetChild(1).gameObject;
        row2Endpoint = gameObject.transform.GetChild(91).gameObject;
        // 3rd row
        row3Startpoint = gameObject.transform.GetChild(2).gameObject;
        row3Endpoint = gameObject.transform.GetChild(92).gameObject;
        // 4th row
        row4Startpoint = gameObject.transform.GetChild(3).gameObject;
        row4Endpoint = gameObject.transform.GetChild(93).gameObject;
        // 5th row
        row5Startpoint = gameObject.transform.GetChild(4).gameObject;
        row5Endpoint = gameObject.transform.GetChild(94).gameObject;
        // 6th row
        row6Startpoint = gameObject.transform.GetChild(5).gameObject;
        row6Endpoint = gameObject.transform.GetChild(95).gameObject;
        // 7th row
        row7Startpoint = gameObject.transform.GetChild(6).gameObject;
        row7Endpoint = gameObject.transform.GetChild(96).gameObject;
        // 8th row
        row8Startpoint = gameObject.transform.GetChild(7).gameObject;
        row8Endpoint = gameObject.transform.GetChild(97).gameObject;
        // 9th row
        row9Startpoint = gameObject.transform.GetChild(8).gameObject;
        row9Endpoint = gameObject.transform.GetChild(98).gameObject;
        // 10th row
        row10Startpoint = gameObject.transform.GetChild(9).gameObject;
        row10Endpoint = gameObject.transform.GetChild(99).gameObject;

        // 1st column (ISSUE: If col1Startpoint is same as row1StartPoint, top row will not show...Fixing this by drawing col1 separately)
        col1Startpoint = Instantiate(empty, row1Startpoint.transform.position, Quaternion.identity);
        col1Endpoint = Instantiate(empty, row10Startpoint.transform.position, Quaternion.identity);

        //// Make instantiated points children of overall grid model (for easy deletion)
        //col1Startpoint.transform.parent = gameObject.transform;
        //col1Endpoint.transform.parent = gameObject.transform;

        // 2nd column
        col2Startpoint = gameObject.transform.GetChild(10).gameObject;
        col2Endpoint = gameObject.transform.GetChild(19).gameObject;
        // 3rd column
        col3Startpoint = gameObject.transform.GetChild(20).gameObject;
        col3Endpoint = gameObject.transform.GetChild(29).gameObject;
        // 4th column
        col4Startpoint = gameObject.transform.GetChild(30).gameObject;
        col4Endpoint = gameObject.transform.GetChild(39).gameObject;
        // 5th column
        col5Startpoint = gameObject.transform.GetChild(40).gameObject;
        col5Endpoint = gameObject.transform.GetChild(49).gameObject;
        // 6th column
        col6Startpoint = gameObject.transform.GetChild(50).gameObject;
        col6Endpoint = gameObject.transform.GetChild(59).gameObject;
        // 7th column
        col7Startpoint = gameObject.transform.GetChild(60).gameObject;
        col7Endpoint = gameObject.transform.GetChild(69).gameObject;
        // 8th column
        col8Startpoint = gameObject.transform.GetChild(70).gameObject;
        col8Endpoint = gameObject.transform.GetChild(79).gameObject;
        // 9th column
        col9Startpoint = gameObject.transform.GetChild(80).gameObject;
        col9Endpoint = gameObject.transform.GetChild(89).gameObject;
        // 10th column // (ISSUE: If col10Startpoint is same as row1Endpoint, bottom row will not show...Fixing this by dreating empty point (col10Startpoint) at same position as row1Endpoint)
        col10Startpoint = Instantiate(empty, row1Endpoint.transform.position, Quaternion.identity);
        col10Endpoint = Instantiate(empty, row10Endpoint.transform.position, Quaternion.identity);

        // Make instantiated points children of overall grid model (for easy deletion)
        col1Startpoint.transform.parent = gameObject.transform;
        col1Endpoint.transform.parent = gameObject.transform;
        col10Startpoint.transform.parent = gameObject.transform;
        col10Endpoint.transform.parent = gameObject.transform;

        // Only enable line renderer on grid intersection points that need them (only left and top edges)
        GameObject[] gridEdgePointsArr = { gameObject.transform.GetChild(0).gameObject, gameObject.transform.GetChild(1).gameObject, gameObject.transform.GetChild(2).gameObject, gameObject.transform.GetChild(3).gameObject, gameObject.transform.GetChild(4).gameObject, gameObject.transform.GetChild(5).gameObject, gameObject.transform.GetChild(6).gameObject, gameObject.transform.GetChild(7).gameObject, gameObject.transform.GetChild(8).gameObject, gameObject.transform.GetChild(9).gameObject, gameObject.transform.GetChild(10).gameObject, gameObject.transform.GetChild(20).gameObject, gameObject.transform.GetChild(30).gameObject, gameObject.transform.GetChild(40).gameObject, gameObject.transform.GetChild(50).gameObject, gameObject.transform.GetChild(60).gameObject, gameObject.transform.GetChild(70).gameObject, gameObject.transform.GetChild(80).gameObject, col1Startpoint, col10Startpoint };
        foreach (GameObject gridEdgePoint in gridEdgePointsArr)
        {
            gridEdgePoint.GetComponent<LineRenderer>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // These points are essentially the same. Used to prevent lines from not showing if for example, col1Startpoint is same as row1Startpoint (both are child 0)
        col1Startpoint.transform.position = row1Startpoint.transform.position;
        col1Endpoint.transform.position = row10Startpoint.transform.position;
        col10Startpoint.transform.position = row1Endpoint.transform.position;
        col10Endpoint.transform.position = row10Endpoint.transform.position;

        // Sets start and endpoints of line renderer at appropriate points on line (Draws column and row lines)
        // Rows
        row1Startpoint.GetComponent<LineRenderer>().SetPosition(0, row1Startpoint.transform.position);
        row1Startpoint.GetComponent<LineRenderer>().SetPosition(1, row1Endpoint.transform.position);

        row2Startpoint.GetComponent<LineRenderer>().SetPosition(0, row2Startpoint.transform.position);
        row2Startpoint.GetComponent<LineRenderer>().SetPosition(1, row2Endpoint.transform.position);

        row3Startpoint.GetComponent<LineRenderer>().SetPosition(0, row3Startpoint.transform.position);
        row3Startpoint.GetComponent<LineRenderer>().SetPosition(1, row3Endpoint.transform.position);

        row4Startpoint.GetComponent<LineRenderer>().SetPosition(0, row4Startpoint.transform.position);
        row4Startpoint.GetComponent<LineRenderer>().SetPosition(1, row4Endpoint.transform.position);

        row5Startpoint.GetComponent<LineRenderer>().SetPosition(0, row5Startpoint.transform.position);
        row5Startpoint.GetComponent<LineRenderer>().SetPosition(1, row5Endpoint.transform.position);

        row6Startpoint.GetComponent<LineRenderer>().SetPosition(0, row6Startpoint.transform.position);
        row6Startpoint.GetComponent<LineRenderer>().SetPosition(1, row6Endpoint.transform.position);

        row7Startpoint.GetComponent<LineRenderer>().SetPosition(0, row7Startpoint.transform.position);
        row7Startpoint.GetComponent<LineRenderer>().SetPosition(1, row7Endpoint.transform.position);

        row8Startpoint.GetComponent<LineRenderer>().SetPosition(0, row8Startpoint.transform.position);
        row8Startpoint.GetComponent<LineRenderer>().SetPosition(1, row8Endpoint.transform.position);

        row9Startpoint.GetComponent<LineRenderer>().SetPosition(0, row9Startpoint.transform.position);
        row9Startpoint.GetComponent<LineRenderer>().SetPosition(1, row9Endpoint.transform.position);

        row10Startpoint.GetComponent<LineRenderer>().SetPosition(0, row10Startpoint.transform.position);
        row10Startpoint.GetComponent<LineRenderer>().SetPosition(1, row10Endpoint.transform.position);


        // Columns
        col1Startpoint.GetComponent<LineRenderer>().SetPosition(0, col1Startpoint.transform.position);
        col1Startpoint.GetComponent<LineRenderer>().SetPosition(1, col1Endpoint.transform.position);

        col2Startpoint.GetComponent<LineRenderer>().SetPosition(0, col2Startpoint.transform.position);
        col2Startpoint.GetComponent<LineRenderer>().SetPosition(1, col2Endpoint.transform.position);

        col3Startpoint.GetComponent<LineRenderer>().SetPosition(0, col3Startpoint.transform.position);
        col3Startpoint.GetComponent<LineRenderer>().SetPosition(1, col3Endpoint.transform.position);

        col4Startpoint.GetComponent<LineRenderer>().SetPosition(0, col4Startpoint.transform.position);
        col4Startpoint.GetComponent<LineRenderer>().SetPosition(1, col4Endpoint.transform.position);

        col5Startpoint.GetComponent<LineRenderer>().SetPosition(0, col5Startpoint.transform.position);
        col5Startpoint.GetComponent<LineRenderer>().SetPosition(1, col5Endpoint.transform.position);

        col6Startpoint.GetComponent<LineRenderer>().SetPosition(0, col6Startpoint.transform.position);
        col6Startpoint.GetComponent<LineRenderer>().SetPosition(1, col6Endpoint.transform.position);

        col7Startpoint.GetComponent<LineRenderer>().SetPosition(0, col7Startpoint.transform.position);
        col7Startpoint.GetComponent<LineRenderer>().SetPosition(1, col7Endpoint.transform.position);

        col8Startpoint.GetComponent<LineRenderer>().SetPosition(0, col8Startpoint.transform.position);
        col8Startpoint.GetComponent<LineRenderer>().SetPosition(1, col8Endpoint.transform.position);

        col9Startpoint.GetComponent<LineRenderer>().SetPosition(0, col9Startpoint.transform.position);
        col9Startpoint.GetComponent<LineRenderer>().SetPosition(1, col9Endpoint.transform.position);

        col10Startpoint.GetComponent<LineRenderer>().SetPosition(0, col10Startpoint.transform.position);
        col10Startpoint.GetComponent<LineRenderer>().SetPosition(1, col10Endpoint.transform.position);
    }
}
